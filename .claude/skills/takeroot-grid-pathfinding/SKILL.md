---
name: takeroot-grid-pathfinding
description: >-
  Пространственный слой Take Root: целочисленная сетка (1 юнит = 1 клетка), проходимость/
  occupancy, ТРИ сосуществующих A*-пасфайндера, тайлмап, туман войны, слои-этажи, host-only
  генерация уровня и её сетевой детерминизм. Используй при запросе/мутации проходимости,
  выборе/отладке пасфайндера, работе с тайлмапом, туманом, слоями, генерацией уровня, и при
  вопросах детерминизма пространственного состояния по сети. Triggers: grid, сетка, walkability,
  проходимость, occupancy, pathfinding, пасфайндинг, A*, AStar, GridService, tilemap, тайлмап,
  fog of war, туман войны, layers, слои, level generation, генерация уровня, Node, координаты.
---

# Take Root — сетка, пасфайндинг, мир

> Сначала `takeroot-project-context`. Кластер «коварный»: несколько пасфайндеров, две абстракции occupancy, координаты в стадии рефактора. Уточни выбор источника истины у Гоши (открытый вопрос).

## Координаты

- **1 мировой юнит = 1 клетка сетки**, без масштабирования.
- `GridObject.Init` — floor-to-int. ⚠️ `Gridable.VectorUtils.ToVector2Int` **флорит X и СЕЙЛИТ Y** (асимметрия → источник off-by-one).
- **Три типа координат, не взаимозаменяемы:** `int3` (живая сетка/`AStar`), `Vector2Int`+`Node` (legacy `AStarPathfinding`), `Point`+`NodeData` (`AStarPathfindingVertical`). Каждый завязан на свой источник occupancy.

## Два источника occupancy

- `MapFromSceneObjects` (`Dictionary<int3,bool>`, сканится из `GridObject`) → питает `GridService` + `AStar`.
- `Gridable` (`Vector2Int`-футпринты, `IsBlockingPath`/`IsBlockingView`) → питает legacy-пасфайндеры + туман.

Предпочитай `IGridService`, но знай, что муверы могут ходить через `Gridable`-пасфайндеры.

## Три пасфайндера — какой актуальный (разрешено по истории коммитов)

**Живой и самый свежий — int3 `AStar`** (`Pathfinder/AStar/AStar.cs`, `SimpleGraph`/`int3`): создан 2025-07-04, реальная работа до 2025-09-01, зарегистрирован как DI `IPathfindService`. **Это источник истины вперёд.**

Legacy (подключены через `ObsoleteCoreEntryPoint`, реальной работы нет — только mass-format 2025-08-26):
- `GridHelpers/AStarPathfinding` (`Vector2Int`, создан 2025-01-11) — зомби/мульти-таргет.
- `GridHelpers/AStarPathfindingVertical` (`Point`/3D, слои+варпы, создан 2025-05-27) — движение поселенца/зомби/зданий.
- `Pathfinder/AStar/IterativeDeepeningAStar` — не зарегистрирован, мёртв.

> ⚠️ **Нюанс вертикальности:** этажи/слои добавлялись в мае (`AStarPathfindingVertical`), но июльский int3 `AStar` (новее) их **не несёт** — `GridService` хардкодит `z=0`. Сейчас вертикальность живёт только в legacy-вертикальном пасфайндере. Понадобится вперёд — переносить в int3-стек (Гоша: «решим по ходу»).

## Запрос проходимости (правильно)

- Гарди `IGridService.OnMap(pos)` **до** `IsOccupiedPos(pos)` (иначе `KeyNotFoundException`). `true` = занято/заблокировано.
- Legacy-альтернативы: `AStarPathfinding.IsWalkable`, `Gridable.IsBlockingPath`.
- ⚠️ `FindPath` возвращает `null` и при throttling, и при отсутствии пути — **null-чек обязателен** (Mover может разыменовать null).

## Мутация сетки

- `IGridService.OccupyTile`/`FreeTile`, `GridObject.OccupyTiles`/`Destroy`.
- ⚠️ **Перф:** каждый `OccupyTile`/`FreeTile` пересобирает **весь** `SimpleGraph` (объект WxH → W*H полных пересборок). Инкрементального апдейта нет — батчи правок дороги.

## Порядок DI (`ServiceLocatorLoader_Main`)

Mock `IPathfindService` → затем `AStar` (после `CreateMap`/`CreateSimpleGraph`). Карта строится до `AStar`/`GridService`. Legacy `GridManager`/`AStarPathfinding(Vertical)`/`WarpManager`/`LayerManager` используют статический `ObsoleteCoreEntryPoint`-локатор.

## Тайлмап

`TilemapType` + `IsMain`. Поток объединения оркестрируется через `ClientRpc` `NetworkDataHolder` с захардкоженным ожиданием 0.5с (`//TODO`, race-prone, неидемпотентно). Туман-тайлмапы (`FogOfWarBlack`/`Grey`) находятся по типу. Кастомные `RuleTile`. ⚠️ Стены живут **и** на тайлмапе, **и** как `Gridable`/`WallTile` — удалять в обоих местах.

## Туман войны

Клиент-локальный, **по-расовый, не сетевой**. Использует `Gridable.IsBlockingView` (legacy-источник). `RefreshWalls` выключен. `FogRect`/`GridSize` должны примерно совпадать. Дебаг-инпут вшит в сервис.

## Слои-этажи

`WorldLayer`/`HasLayer`/`LayerRenderer`/`LayerManager` + варпы. ⚠️ Только `AStarPathfindingVertical` учитывает ось z/слой; `GridService` хардкодит `z=0`. Формула `sortingOrder` хрупкая для слоя 0/отрицательных.

## Генерация уровня — ЕЁ ЕЩЁ НЕТ (писать с нуля)

> Подтверждено Гошей: **настоящей процедурной генерации уровня пока нет** — её предстоит написать. Намеченная модель: **хост генерирует уровень и реплицирует клиенту**.

Что есть сейчас (`Infrastructure/LevelGeneration/LevelGenerationService.cs`) — **плейсхолдер**, вызывается host-only из `CoreEntryPoint.OnNetworkPostSpawn`:
- комбинирует **пред-нарисованные** тайлмапы (`ClearAndCombineTilemaps` + `…ClientRpc`);
- рандомит декор по сиду (`GenerateRandomDecor(seed)` → `decor.Init(seed)`, host + `…ClientRpc`);
- рандомит имена поселенцев (`GenerateSettlers`) — host-only, имена **реплицируются** через `UpdateNamesDataClientRpc` (т.е. имена синхронны, десинка нет — поправка к аудиту);
- `//TODO` с захардкоженным ожиданием `WaitForSeconds(0.5f)` вместо нормальной синхронизации инициализации.

**Когда будешь писать настоящую генерацию** (host-authoritative → реплика клиенту):
- ⚠️ Не полагайся на `GetInstanceID()` для сидирования — он **различается** у хоста и клиента. Сейчас `RandomDecorObject`/`RandomView` рискуют десинком декора; делай host-authoritative с явной репликацией результата по RPC (как уже сделано с именами), а не «одинаковый сид на обоих пирах».
- Замени `WaitForSeconds(0.5f)` на корректное ожидание готовности.

## Overlays

⚠️ `OverlayService` — заглушка (no-op). Enum/презентер/`ReactiveProperty` есть; реальный оверлей требует рендера в `OverlayService` (+ вероятно отдельный тайлмап/рендерер).

## Известные баги (проверить/чинить)

- Границы `max.x - min.y` в `GridManager.FillGrass`, `FogOfWarService.Fill`.
- Инклюзив (`<=`) футпринт в `CreateMap` vs эксклюзив (`<`) `GridObject.IsObstacle`.
- `AStarPathfinding.InitializeGrid` баг `y = min.x`.
- Асимметричное округление `ToVector2Int`.
- `AStarPathfindingVertical`: аллокация 1000×1000×4 + полный `Array.Clear` на каждый запрос.
