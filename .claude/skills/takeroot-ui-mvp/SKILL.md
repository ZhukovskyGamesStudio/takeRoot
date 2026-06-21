---
name: takeroot-ui-mvp
description: >-
  UI Take Root: uGUI + TextMeshPro, паттерн MVP (пассивный View + plain-C# Presenter,
  создаваемый в CoreEntryPoint.InitPresenters), реактивщина на UniRx, биндинг через
  SerializedDictionary, toggle-группы открытия панелей, и сетевой сплит (большинство UI
  клиент-локальны, но время/исследования идут через RPC). Используй при добавлении/правке
  любой UI-панели, презентера, вью, выделения, инфо-панели, нотификаций, контролов времени/
  исследований, меню, UI камеры. Triggers: UI, панель, panel, View, Presenter, MVP, меню,
  menu, selection, выделение, info panel, notifications, нотификации, time, время, research,
  toggle, CoreCanvasUi, InitPresenters.
---

# Take Root — UI (uGUI + MVP)

> Сначала `takeroot-project-context`. Стек — **uGUI + TextMeshPro только**; UI Toolkit в геймплее нет. ⚠️ `UIElementsSchema` в корне репо — это редакторные XSD, игнорируй.

## MVP везде

- **View** = пассивный MonoBehaviour: `[SerializeField]`-виджеты + методы `SetData`/`Init` (тупой рендер).
- **Presenter** = plain-C# класс, **создаётся в `CoreEntryPoint.InitPresenters()`** (точка регистрации), держит сервисы через `ServiceLocator.Single<T>()`, подписывается на реактивное состояние, часто `IUpdatable` + `IDisposable`.

## Рецепт: добавить панель

1. Добавь значение в `enum PanelType`.
2. В сцене: GameObject панели → `CoreCanvasUi.PanelsView.Panels`; `Toggle` → `PanelTogglesView.Toggles`; соответствующую запись `ToggleData` (требования по Research/Race).
3. Создай `View` (`SetData`/`Init`) + plain-C# `Presenter`.
4. Сошлись на `View` из `CoreCanvasUi` или `CoreEntryPoint`.
5. Создай `Presenter` в `CoreEntryPoint.InitPresenters`, прокинув `Single<IService>()`-зависимости.
6. Расово-вариативный визуал — через `IHasRaceVariant`, **не** ветвлением по расе.

## Конвенции

- Открытие/закрытие: `SerializedDictionary<enum, Toggle/GameObject>` + `Toggle.onValueChanged` + `GameObject.SetActive`.
- Панели **взаимоисключающи**: открытие панели снимает выделение, выделение закрывает панели.
- Видимость toggle гейтится `ToggleData` (`ResearchRequirement`/`RaceRequirement`) через `UpdatePanels`.

## Реактивщина

- UniRx `ReactiveProperty` для выделения / pending-команды / готовности времени / ingame-времени.
- Plain `Action`-события для сигналов (`OnResearchFinished`, `NewNotification`, `OnJobChanged`).
- Пер-кадровый рефреш — `IUpdatable`+`IUpdateService` или `Update`/`FixedUpdate` во View. ⚠️ Следи за пер-кадровым LINQ/аллокациями (горячая точка — `TimeStatusUI` colony stress).

## Сетевой сплит UI

- **Клиент-локальные:** панели, выделение, инфо, камера, нотификации, оверлеи, ingame-часы.
- **Сетевые:**
  - **Скорость/пауза** — `GameSpeedView` → `ServerRpc` → `NetworkDataHolder.SetGameSpeedClientRpc`, выставляет глобальный `Time.timeScale` (**побеждает минимум из двух игроков**; пауза гейтится заряженной `TimeMachine`).
  - **Исследования** — `NetworkDataHolder.ResearchNetworkData`.
- ⚠️ `NetworkDataHolder.Instance` должен быть заспавнен **до** вызовов времени/исследований (иначе NRE). См. `takeroot-netcode`.

## Время

`Time.timeScale` — единственные синхронизируемые часы (глобально через `ClientRpc`). ⚠️ UI, который должен работать на паузе, нужен на `unscaledDeltaTime` (по большей части не сделано). `IngameTimeService` — **по-клиентный** (не сетевой), масштабируется с `timeScale`.

## Выделение → инфо

- `ISelectionService.SelectedReactive` — единый источник истины.
- `SelectionServicePresenter` type-switch'ит `SettlerSelectable` vs `CommandTargetSelectable`. ⚠️ Безусловный каст к `InfoDataCombined` может бросить.
- `SettlerInfoPanel` свапает render-слой + портрет-камеру.
- Выделение **выключено**, пока висит pending `JobType`-команда.

## Известные опасные места

> ⚠️ требует проверки у Гоши.

- ⚠️ `Dispose()` презентеров часто пуст/не вызывается → подписки живут весь lifetime сцены, утекают при перезагрузке.
- ⚠️ Смешан legacy `Input` и новый Input System (камера); `EventSystem.current` разыменовывается без null-чека.
- `OverlayService` — заглушка (см. `takeroot-grid-pathfinding`).
- `CoreCanvasUi.OpenInfoPanel` — `[Obsolete]` (используй `SelectionServicePresenter`).
- Строки RU/EN захардкожены, локализации нет.
- Кнопки меню провязаны через инспектор `onClick` — открой префаб, чтобы найти вызовы. `MenuEntryPoint.Play` — host-only.
