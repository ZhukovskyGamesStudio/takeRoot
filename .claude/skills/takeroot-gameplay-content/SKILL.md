---
name: takeroot-gameplay-content
description: >-
  Добавление и правка игрового контента Take Root: здание, рецепт крафта, растение, тип
  ресурса, occurence (явление/волна), квест, шутер/турель, электрический объект. Data-driven
  паттерн ScriptableObject + IConfigsProvider, host-authoritative правила, карта live-vs-legacy
  по геймплей-папкам. Используй при добавлении/правке геймплей-контента. Triggers: building,
  здание, crafting, рецепт, recipe, farming, растение, plant, resource, ресурс, occurence,
  явление, quest, квест, turret, турель, electricity, контент, IConfigsProvider, config.
---

# Take Root — игровой контент

> Сначала `takeroot-project-context`. Это большой кластер с «мёртвыми двойниками» — сверяйся со стеком перед правкой. Единая точка регистрации всех SO-списков — **`IConfigsProvider`**. Общий сетевой паттерн: `ServerRpc(RequireOwnership=false)` → мутация на хосте → `ClientRpc`-синк, гейт на `IsHost` (см. `takeroot-netcode`). Разделение хранения SO (`Resources/Configs` vs `Assets/Configs`) — в `takeroot-code-style`.

## Карта live-vs-legacy (контент)

| Система | Живое | Мёртвое |
|---|---|---|
| Building | `BuildingService` / `BuildingBlueprint` / `BuildingRecipeConfig` | `BuildingManager` / `BuildingPlan` |
| Crafting | `CraftingService` / `CraftingStation` / `CraftingRecipeConfig` | `CraftingManager` / `CraftingStationable` |
| Farming | весь новый стек | — |
| Электричество | `Electricity/` (per-object `ElectricityLevel`) | `Power/` (сеть проводов) |
| Ресурсы | `GameResources/ResourcesManager` | `Managers/ResourceManager` |
| Quests | — | целиком legacy (`QuestManager`/`GameEventsManager`) |
| Shoot/Tactical | — | legacy ECS (`ECSEntity`, привязка к `ObsoleteCoreEntryPoint`) |

## Добавить здание

Создай `BuildingRecipeConfig` (меню `Scriptable Objects/BuildingRecipeConfig`) в `Resources/Configs/BuildingRecipes`: `mainInfo`, `BuildingPrefab` (префаб с `CommandTarget`), `Footprint`, `RequiredBuildPoints`, `Ingridients`, `RequiredResearch`. Убедись, что попадает в `IConfigsProvider.BuildingsBlueprintsConfigs`. Размещение **host-authoritative** (early-return при `!IsHost`). Шорткат: `AdminManager.IsInstaBuild`.

## Добавить рецепт крафта

`CraftingRecipeConfig`: `MainInfo`, `CraftingPoints`, `RequiredResources`, `ResultingResource`, `RequiredResearch`. Добавь в `StationData.AvailableCraftingRecipes` станции. Рецепт идентифицируется по `ResultingResource.ResourceType` — **станция не может держать два рецепта на один тип результата**. Результат спавнится в `InteractPos[0]` через `IResourceManager`. Хранение по GUID → `Assets/Configs/Crafting Recipes/<Station>/`.

## Добавить растение

Добавь в `enum FarmingPlantType`; создай `FarmingPlantConfig` (`PlantType`, `MainData`, `GrowthStages` float→Sprite, `DropOnHarvest`) в `Resources/Configs/FarmingPlants` → `IConfigsProvider.FarmingConfigs`.
- ⚠️ `DryRate`/`GrowRate` захардкожены в `FarmingService`.
- ⚠️ Харвест **не уничтожает** растение (сбрасывает в `Growing`).
- ⚠️ Проверь инвертированную на вид семантику `NeedsWatering` перед правками ИИ.

## Добавить тип ресурса

Добавь в `enum ResourceType` (`WorldObjects/ResourceType.cs`); префаб `Resource` в `ResourcesConfig.ResourcesPrefabs`; иконку в `ResourcesTable`; вес в `ResourcesConfig.ResourceWeights` (если спавнится случайно). Многие системы итерируют `Enum.GetValues` (пропуская `None`), поэтому потребители подхватят автоматически. Стандартная карта количеств — `SerializedDictionary<ResourceType,int>`.

## Добавить occurence (явление)

Occurence = **режиссёр эмерджентных угроз** (не путать с Quest). Добавь `OccurenceType` (если новая категория); создай `OccurenceConfig` (`Type`, `DifficultyCost`, `RandomPrefabToSpawn` с `NetworkBehaviour`) → `IConfigsProvider.OccurenceConfigs`. Host-only режиссёр на бюджете сложности спавнит **первый по порядку** доступный конфиг — **порядок в списке важен**. ⚠️ `PickSpawnPos` может зациклиться; LINQ `Max`/`Min` бросают на пустом списке поселенцев.

## Добавить квест/шаг (LEGACY-стек)

⚠️ Целиком на старом стеке (`QuestManager` = `IInitableInstance` через `ObsoleteCoreEntryPoint`/`GameEventsManager`). `QuestConfig` (меню `Quests/QuestConfig`) в `Resources/Quests`: `ID`, `Race`, `NextQuests`, `QuestStages`→`QuestStepPrefabs`, пререквизиты. Сабкласс `QuestStep`: подпишись на событие `GameEvents` в `Start`, вызови `FinishStep`. Шаги инстанцируются off-map в `(999,0,0)`. `StartQuest("door")` захардкожен. Миграция на новый стек — открытый вопрос к Гоше.

## Добавить шутер/турель или электрический объект (LEGACY ECS)

⚠️ `Shoot/*` и `Tactical/*` — **legacy ECS** на `ObsoleteCoreEntryPoint`. Турель: префаб `TurretEntity`/`ECSEntity` с `Shooter`/`Turret` + `TacticalInteractable` + `Gridable` + `Animator` + спавн-точкой снаряда + `RandomProjectileSelector`. Поток снаряда — через Animator-события; цели захардкожены на `AI.Zombie`. Электрообъект: добавь `ElectricityLevel` (+ опц. `Progress`/`DirectChargeCable`).

## Кросс-режущее

- `IConfigsProvider` — единая регистрация всех SO-списков контента.
- Host-authoritative: `ServerRpc(RequireOwnership=false)` → мутация → `ClientRpc`, гейт `IsHost`.
- Не мешай legacy `ECSComponent` с сетевым `CommandTarget` на одном префабе.
- Избегай пер-кадровых `FindObjectsByType` и рекурсивного flood-fill (перф-смелл).
- ⚠️ `WorldState.GlobalEnergyChangeMultiplier` всегда 0 (потребитель закомментирован) — проверь у Гоши: баг или намеренно.
- ⚠️ `ResearchStation.StopWorking` дублирует `StartWorking` — проверь.
