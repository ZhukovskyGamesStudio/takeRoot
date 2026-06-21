---
name: takeroot-project-context
description: >-
  Контекст проекта Take Root: что за игра, команда, цели, терминология Гоши, и
  главное — авторитетная карта live-vs-legacy (две параллельные архитектуры,
  «мёртвые двойники», одноимённые типы). Используй ПЕРВЫМ перед работой над любой
  подсистемой и всегда, когда неясно: живой файл или legacy, кто что симулирует,
  что означает термин. Triggers: live or legacy, obsolete, ObsoleteCoreEntryPoint,
  Managers, GameEventsManager, ECS, dead twin, Race, Plants, Robots, Occurence,
  явление, что это за игра, о проекте.
---

# Take Root — контекст проекта

## Игра в двух абзацах

Кооперативный сетевой симулятор колонии на двоих (Unity 6, URP, 2D, top-down). Двое игроков совместно управляют поселением: один — **роботами**, второй — **растениями**. Это две `Race`: `Plants` и `Robots`. Сеттинг: город «Н», где случилось «**явление**» — люди потеряли рассудок, а предметы и существа его обрели (отсюда разумные растения-поселенцы, ожившая мебель, зомби-люди).

Геймплей сессионный, из двух частей: **хаб** (основное поселение) и **вылазки** (аванпосты). Каждую вылазку игроки выбирают клетку на карте города — карта и задачи миссии генерируются процедурно. Игроки раздают команды поселенцам (gather/build/craft/farm/defend), кооперируясь между двумя расами.

## Команда и стадия

- Один программист — **Гоша**. Будущая команда подключится после готовности основных фич.
- Текущая цель — **MVP** для привлечения инвестиций, аудитории и найма фуллтайм-команды.
- ГДД в активной разработке: пока готовы только общие идеи; детали будут уточняться.
- Это **долгострой**. Приоритеты: порядок, консистентность, читаемость. Не плоди новый хаос; следуй уже заведённым конвенциям (см. `takeroot-code-style`).

## ⚠️ Две архитектуры в одном репозитории

Самое важное знание о кодовой базе: **актуальный стек сосуществует с большим legacy-стеком**, который ещё частично подключён, но идёт под снос. Почти у каждой крупной системы есть **«мёртвый двойник»**. Прежде чем читать/править файл — определи, на каком он стеке.

### Как отличить живое от legacy

**Живое:**
- Класс реализует `IService` и регистрируется в `ServiceLocatorLoader_Main.RegisterServices()`.
- Резолвится через `ServiceLocator.Container.Single<IXxx>()`.
- ИИ — через `BTNode` (`AI/Node`), команды игрока — через `JobType` (`AI/Node/Jobs/Jobs.cs`), контент — через `ScriptableObject` + `IConfigsProvider`.

**Legacy (НЕ расширять, кандидаты на удаление):**
- `[Obsolete]` `ObsoleteCoreEntryPoint` (статический, ~20 manager-ссылок, топо-сортировка `IInitableInstance`).
- Папка `Managers/` (статические синглтоны-менеджеры) — **кроме `AdminManager`, см. ниже**.
- `GameEvents/` (`GameEventsManager`, `WorldObjectsEvents`) — старая шина событий.
- Кастомный «ECS»: `ECSEntity` / `ECSComponent` (`WorldObjects/EcsSystem/`).
- Старая система команд: `BaseCommand`, `ConcreteCommands/*`, `Worker`, `WorkerAssigner`.
- `Power/` (сеть проводов flood-fill), `Video/` (демо-сцены).

### Карта «живое ↔ мёртвый двойник»

| Система | Живое | Мёртвый двойник |
|---|---|---|
| Электричество | `Electricity/` (per-object `ElectricityLevel` заряд) | `Power/` (сеть проводов, `PowerManager`) |
| Ресурсы | `GameResources/ResourcesManager` (`IResourceManager`) | `Managers/ResourceManager` (статик) |
| Строительство | `BuildingService` / `BuildingBlueprint` / `BuildingRecipeConfig` | `BuildingManager` / `BuildingPlan` |
| Крафт | `CraftingService` / `CraftingStation` / `CraftingRecipeConfig` | `CraftingManager` / `CraftingStationable` |
| Поселенцы | `AI.Settler` + дерево поведения | `WorldObjects/Creatures/Settler` (ECS) |
| Команды | `JobType` + `CommandTarget` + capabilities | `BaseCommand` / `ConcreteCommands` / `Worker` / `CommandType` |
| Транспорт сети | `UnityTransport` (прямой IP) | `SteamNetworkTransport` (legacy P2P, AppId-заглушка 480) |
| Квесты | — | `Quests/` целиком на legacy (`QuestManager` = `IInitableInstance` через `ObsoleteCoreEntryPoint`/`GameEventsManager`) |

> Решение Гоши: всё перечисленное в правой колонке считается **мёртвым и под снос** — не расширять, при возможности удалять. Квесты пока живут на старом стеке: новые квесты тоже идут через legacy, пока миграция не запланирована (см. «Открытые вопросы»).

## AdminManager — мост между стеками

`Managers/AdminManager.cs` лежит в legacy-папке, но это **живой** и важный объект:
- Режим **`IsFakeOnline`** — одиночная отладка без второго клиента: поднимает host-сессию, фейкает ready-флаги, назначает `HostRace=Plants`/`ClientRace=Robots`, и **OR'ится в `IsHost`/`IsOwner`**.
- Любой сетевой код должен корректно работать при `IsFakeOnline`.
- Есть и другие дев-шорткаты: `IsInstaBuild`, `IsGodmode` и т.п.

## Одноимённые типы — как не перепутать

Есть несколько типов с одинаковыми именами в разных namespace/папках. **Активен `AI.*`:**
- `Settler` — живой `AI.Settler`; мёртвый `WorldObjects/Creatures/Settler`.
- `Zombie`, `SettlerData` — аналогично, ориентируйся по namespace/папке.
- `ResourceManager` — живой `GameResources` (`IResourceManager`); мёртвый `Managers`.

Различай по **namespace и папке**. Если файл без namespace и лежит в `Managers/`/`WorldObjects/EcsSystem/`/`GameEvents/`/`Power/` — почти наверняка legacy.

## Словарь терминов

- **Race** — идентичность игрока: `None` / `Plants` / `Robots` / `Both`. Это сетевая «роль», не ownership.
- **Occurence** (рус. «явление», в коде одна `r`; папка конфигов `Configs/Occurencies`) — режиссёр эмерджентных угроз/событий (волны, инциденты). **Не путать с Quest** — это разные системы.
- **Quest** — скриптовые поэтапные цели (legacy-стек).
- **Job** — у термина два живых механизма: реестровый `JobType` (через `ICommandService`) и напрямую добавляемые `Job_*` BT-узлы. См. `takeroot-commands`.
- **Settler** — поселенец (юнит игрока). **Worker** — legacy-понятие из старой системы команд, не использовать.

## Золотые правила для новой работы

1. Никогда не расширяй `[Obsolete]`-пути и мёртвых двойников.
2. Новый сервис → через `ServiceLocator` + `IService` (`takeroot-bootstrap-and-di`).
3. Новый ИИ → через `BTNode` (`takeroot-ai-behavior-trees`).
4. Новая команда игрока → через `JobType` (`takeroot-commands`).
5. Новый контент → `ScriptableObject` + `IConfigsProvider` (`takeroot-gameplay-content`).
6. Сначала определи стек файла, потом правь.

## Индекс кросс-системных опасных мест

> ⚠️ = требует подтверждения Гоши (возможна ошибка чтения аудита). Детали — в скилле-владельце.

- ⚠️ `IGameFactory` потребляется, но не зарегистрирован — недописанный/заброшенный путь (потребитель `TacticalMergable` — legacy ECS) → `takeroot-bootstrap-and-di`.
- `ServiceLocator` статический, не сбрасывается между сессиями → `takeroot-bootstrap-and-di`.
- `NetworkVariable<SettlerData>` сериализует только `curMovePos` → `takeroot-netcode`.
- Генерации уровня по сути ещё нет — `LevelGenerationService` плейсхолдер, писать с нуля (хост → реплика клиенту) → `takeroot-grid-pathfinding`.
- ⚠️ `WorldState.GlobalEnergyChangeMultiplier` всегда 0 → `takeroot-gameplay-content`.
- ⚠️ `ResearchStation.StopWorking` дублирует `StartWorking` → `takeroot-gameplay-content`.
- `IdentifierService` ids локальны, **не сетевые** — не использовать для кросс-пир идентичности → `takeroot-bootstrap-and-di`.

## Статус открытых вопросов

Часть разрешена по коду/истории коммитов (июнь 2026); часть Гоша после года перерыва не помнит — помечено как требующее изучения. Не домысливай по непомеченному.

**Разрешено:**
- **Актуальный пасфайндер** — int3 `AStar` (`IPathfindService`), самый свежий (создан июль, работа до сен 2025); `GridHelpers/AStarPathfinding(Vertical)` — legacy через `ObsoleteCoreEntryPoint`. Вертикальность пока только в legacy-вертикальном. См. `takeroot-grid-pathfinding`.
- **Генерация уровня** — настоящей нет, текущий `LevelGenerationService` — плейсхолдер. Писать с нуля; модель: хост генерит → реплика клиенту. См. `takeroot-grid-pathfinding`.
- **`Power/` vs `Electricity/`** — `Electricity/` активно дорабатывается (последняя работа сен 2025), `Power/` трогали только mass-format → `Power/` считаем мёртвым (Гоша точно не помнит — ревью при удалении).

**Не решено / отложено:**
- **Владение поселенцами** — Гоша: «решим по ходу». Пока хост симулирует все деревья; новый ИИ-код считай host-исполняемым.
- **Полный план удаления legacy** и миграция квестов — Гоша не помнит (год перерыва); перед удалением чего-либо крупного нужно изучение кода. До ревью legacy **не расширять, но и не удалять массово вслепую**.
- **`IGameFactory`** — недописанный/заброшенный путь (см. индекс выше); замысел Гоша не помнит.
- **Steam** — транспорт/лобби в роадмапе или прямой IP UTP? Не решено.
- **Локализация** — RU/EN захардкожены инлайн; слой локализации не решён.
- **`WorldState.GlobalEnergyChangeMultiplier == 0`, `ResearchStation.StopWorking`** — Гоша не помнит; проверить при работе с электричеством/исследованиями.
