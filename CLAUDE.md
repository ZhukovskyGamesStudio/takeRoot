# Take Root — гид по проекту для Claude

> Этот файл — оглавление и карта проекта. Глубина по каждой подсистеме вынесена в **скиллы** (`.claude/skills/`). Здесь — только то, что нужно знать всегда, и указатели куда идти за деталями.

## Что это за игра

**Take Root** — кооперативный симулятор колонии на двоих по сети (Unity 6, URP, 2D, вид сверху). Двое игроков управляют поселением: один — **роботами**, второй — **растениями** (это две `Race` — `Plants` / `Robots`). Они исследуют город «Н», где произошло «явление»: люди потеряли рассудок, а предметы и существа его обрели. Геймплей сессионный: управление хабом (основное поселение) + вылазки на процедурно генерируемые клетки города за ресурсами.

Проект ведёт один программист (Гоша). Цель — MVP для привлечения инвестиций и команды. Это долгострой: **порядок, консистентность и читаемость важнее скорости**.

## ⚠️ Правило №1: сначала пойми, на каком стеке файл

В репозитории **параллельно живут две архитектуры**: актуальная и большой ещё-подключённый **legacy-слой**, который под снос. У многих систем есть «мёртвый двойник». Расширять legacy вместо живой системы — самая дорогая ошибка здесь.

- **Живое** = реализует `IService`, регистрируется в `ServiceLocatorLoader_Main`, резолвится через `ServiceLocator.Container.Single<T>()`.
- **Legacy** = `[Obsolete]` `ObsoleteCoreEntryPoint` + `Managers/` (синглтоны) + `GameEventsManager` + кастомный «ECS» (`ECSEntity`/`ECSComponent`) + старые `BaseCommand`/`Worker` + `IInitableInstance`.

Полная карта live-vs-legacy и как различать одноимённые типы — в скилле **`takeroot-project-context`**. Читай его **первым** перед работой над любой подсистемой.

## Стек

- **Unity 6** (`6000.3.9f1`), **URP 17** (2D). Сборка — `Assembly-CSharp` целиком (нет `.asmdef` в `Assets/Scripts`).
- **Сеть:** Netcode for GameObjects (NGO) поверх `UnityTransport` (прямой IP). Модель строго **host-authoritative**.
- **Async:** Cysharp **UniTask** (через `IAsyncRunner`). **Реактивность:** **UniRx** `ReactiveProperty` (вендорится в `Assets/Plugins/UniRx`).
- **Инспектор-словари:** AYellowpaper `SerializedDictionary`.
- **DI:** свой статический `ServiceLocator` (НЕ Zenject/VContainer).
- **Загрузка ресурсов:** только Unity `Resources` (Addressables нет).
- **Плагины:** ParrelSync (мульти-эдитор для теста сети), Steamworks.NET (присутствует, но **не подключён** — Steam-транспорт/лобби мёртвы), Input System, Cinemachine, Tilemap.
- C# **без** nullable reference types. Конвенции кода — в скилле **`takeroot-code-style`**.

## Карта архитектуры

```
Boot:  EntryPointBase сцены  →  MenuScene → LoadingScene → MenuScene → CoreScene
                                                                          │
CoreEntryPoint.Awake  =  композиционный корень:                          ▼
   ServiceLocatorLoader_Main.RegisterServices()   →  ~30 сервисов (IService)
   заполнение DataProvider
   InitPresenters()                               →  MVP-презентеры UI

Поток геймплея:
   клик игрока → CommandTarget (ServerRpc) → ICommandService (реестр джоб, на хосте)
              → дерево поведения поселенца (AI/Node) → capability-компонент → сетевой эффект (ClientRpc)
```

Слои рантайма: (1) сервисы `Infrastructure/ServiceLocator`; (2) тик-цикл `IUpdateService`/`IUpdatable` + UniTask; (3) host-authoritative сеть через `NetworkDataHolder`; (4) геймплей-сервисы (Building/Crafting/Farming/Resources/Occurences/Tactical/Research); (5) деревья поведения ИИ → capability-компоненты; (6) пространственный слой Grid/Pathfinding/FogOfWar/Layers; (7) UI на uGUI (MVP).

## Ключевые директории (`Assets/Scripts/`)

| Папка | Назначение | Статус | Скилл |
|---|---|---|---|
| `Infrastructure/` | boot, `ServiceLocator`, конфиги, factory, levelgen | живое (+ подмножество `[Obsolete]`) | `takeroot-bootstrap-and-di` |
| `Online/`, `Steam/` | сеть | `Online` живое (UTP); `Steam` мёртв | `takeroot-netcode` |
| `AI/`, `Settlers/` | деревья поведения, поселенцы, зомби, нужды | живое (`AI.*`) | `takeroot-ai-behavior-trees` |
| `Commands/` | `JobType`/`CommandTarget`/capabilities | живое; `BaseCommand`/`Worker` мёртвы | `takeroot-commands` |
| `Building` `Crafting` `Farming` `Electricity` `GameResources` `Occurences` `Quests` `Tactical` | геймплей-контент | **смешанное** — см. карту в скилле | `takeroot-gameplay-content` |
| `Grid` `Pathfinder` `GridHelpers` `FogOfWar` `Layers` | пространство, A* | живое + legacy-пасфайндеры | `takeroot-grid-pathfinding` |
| `UI/`, `Menu/` | uGUI + MVP | живое | `takeroot-ui-mvp` |
| `Managers/` `GameEvents/` `Power/` `WorldObjects/EcsSystem/` `Video/` | **LEGACY** — не расширять | мёртвое | `takeroot-project-context` |

Корни ассетов: `Assets/Resources` (грузится по пути — `Resources.Load(All)`), `Assets/Configs` (SO по GUID-ссылкам), `Assets/Prefabs`, `Assets/Sprites`, `Assets/Scenes`. Правила организации ассетов — в `takeroot-code-style`.

## Сборка / запуск / тесты

- Открывать проект в **Unity 6** из корня репозитория.
- Боевые сцены: `Scenes/MenuScene` → `Scenes/LoadingScene` → `Scenes/CoreScene` (вход через `MenuScene`).
- **Одиночная отладка:** режим `AdminManager.IsFakeOnline` — поднимает хост, фейкает ready-флаги, назначает `HostRace=Plants`/`ClientRace=Robots`. Так можно итерировать без второго клиента.
- **Тест сети на двоих:** клоны через **ParrelSync** (хост входит по кнопке Play; клиент подключается по IP и едет за хостом через сетевую загрузку сцены).
- **Автотестов как источника истины нет.** `Scenes/Tests/*` — ручные тестовые сцены. Юнит-тесты есть только в `Commands/CommandsSystem/Tests`.
- Доступен **Unity MCP** для автоматизации редактора — см. скилл `unity-mcp-skill`.

## Сеть: три правила (кратко)

Детали — в `takeroot-netcode`. Главное:
1. **Host-authoritative.** Клиент только шлёт `ServerRpc(RequireOwnership=false)`; хост меняет состояние и рассылает `ClientRpc`. Host-only работу гейти на `INetworkService.IsHost` (он же OR'ится с `AdminManager.IsFakeOnline`).
2. **Идентичность игрока = `Race`** (`Plants`/`Robots`) через `NetworkDataHolder`, **никогда** `OwnerClientId`. Хост — условно клиент 1, присоединившийся — клиент 2.
3. **`NetworkVariable<SettlerData>` сериализует только `curMovePos`.** Любое новое реплицируемое состояние поселенца требует явного `ClientRpc`, а не авто-синка.

## Скиллы (что когда читать)

- **`takeroot-project-context`** — *первым делом*, и всегда когда неясно live/legacy, кто что симулирует, или что означает терминология Гоши.
- **`takeroot-code-style`** — при написании/правке любого C#, SO-конфига, префаба или ассета.
- **`takeroot-bootstrap-and-di`** — добавление/правка сервиса, boot-флоу, update-цикл, загрузка конфигов, отладка null-сервисов.
- **`takeroot-netcode`** — любой сетевой код: RPC, `NetworkVariable`, spawn, репликация состояния, дебаг десинков.
- **`takeroot-ai-behavior-trees`** — поведение поселенцев/зомби: новые `Action_`/`Job_`/`Behavior_`, capability-компоненты, нужды.
- **`takeroot-commands`** — как клик игрока превращается в работу поселенца: новый `JobType`, `CommandTarget`, capability.
- **`takeroot-gameplay-content`** — добавить здание/рецепт/растение/ресурс/явление/квест и понять data-driven паттерн `IConfigsProvider`.
- **`takeroot-grid-pathfinding`** — проходимость/occupancy, выбор пасфайндера, тайлмап, туман войны, слои, генерация уровня, детерминизм по сети.
- **`takeroot-ui-mvp`** — любая UI-панель/презентер/вью, выделение, инфо-панели, нотификации, контролы времени/исследований.

## Известные опасные места

> Аудит-находки. Помечены ⚠️ те, что **требуют проверки у Гоши** (могли быть неверно прочитаны). Детали и владелец — в соответствующем скилле.

- ⚠️ `IGameFactory` потребляется (`ResourceSpawnPoint`, `TacticalMergable`), но **не зарегистрирован** — недописанный/заброшенный путь (`TacticalMergable` — legacy ECS), а не активный краш. См. `takeroot-bootstrap-and-di`.
- `ServiceLocator` — статический и **никогда не сбрасывается** между сессиями/перезагрузкой сцены: стейл-сервисы переживают возврат в меню. См. `takeroot-bootstrap-and-di`.
- `IPathfindService` регистрируется дважды (`MockPathfindService`, затем int3 `AStar` — актуальный). См. `takeroot-grid-pathfinding`.
- Настоящей генерации уровня **ещё нет** — `LevelGenerationService` плейсхолдер; писать с нуля (хост → реплика клиенту), не полагаясь на `GetInstanceID()` для сида. См. `takeroot-grid-pathfinding`.
- ⚠️ `WorldState.GlobalEnergyChangeMultiplier` всегда 0 (строка-потребитель закомментирована) — проверить. См. `takeroot-gameplay-content`.

## Память

Долгоживущие факты о пользователе и проекте — в `C:\Users\grafe\.claude\projects\D--Unity-Projects-takeRoot\memory\` (индекс — `MEMORY.md`).
