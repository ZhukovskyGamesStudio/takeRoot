---
name: takeroot-ai-behavior-trees
description: >-
  Кастомное дерево поведения Take Root (AI/Node): код-авторские BT (BTNode/Evaluate/
  BTNodeState), узлы Sequence/Selector/Action_/Job_/Behavior_, fluent-сборка, capability-
  компоненты поселенца (IMovable/IBuilder/...), система нужд, поселенцы и зомби, сетевая
  авторитетность ИИ. Используй при добавлении/правке поведения поселенцев или зомби: новые
  Action/Condition/Job/Behavior, проводка в дерево, capability-компоненты, нужды. Triggers:
  AI, ИИ, behavior tree, дерево поведения, BTNode, Action_, Job_, Behavior_, Settler,
  поселенец, Zombie, зомби, needs, нужды, Selector, Sequence, capability.
---

# Take Root — деревья поведения ИИ

> Сначала `takeroot-project-context`. Живой ИИ — `AI.*`. Legacy `WorldObjects/Creatures/Settler` (ECS) **не трогать**.

## Что это

**Кастомный код-авторский BT** (`AI/Node`) — **не пакет** и не ScriptableObject-авторинг. Деревья собираются в конструкторах `BTRoot_*` / `Behavior_*` / `Job_*` флюентом и **пересоздаются при каждом спавне** в `Settler.CreateRootBt`.

## База узлов

`AI/Node/Node.cs`:
```csharp
public abstract class BTNode {
    public abstract BTNodeState Evaluate();   // override это
}
public enum BTNodeState { Running, Success, Failure }
```
- Композиты: `Sequence`, `Selector`, `ParallelSelector`, `Inverter` — собираются `.AddChild(...)`.
- `Selector`/`Sequence`/`ParallelSelector` **запоминают последнего Running-ребёнка** между тиками (stateful, `_currentChild`).
- ⚠️ `ParallelSelector` назван неверно (он **не** параллельный); его дети невидимы для `BTDebug`.
- Условие инлайн: `ConditionalAction().Do(node).While(() => predicate)`.

Пример (`Job_Build`):
```csharp
var build = new Sequence()
    .AddChild(new Action_FindBlueprintToBuild(settler, buildingService))
    .AddChild(moveToBuilding)
    .AddChild(new Action_Build(settler));
AddChild(new Selector().AddChild(build).AddChild(clear));
```

## Как добавить Action

1. `Action_X.cs` в `AI/Node/Actions[/<домен>]`, наследуй `BTNode`.
2. Конструктор берёт `Settler` (+ нужный сервис).
3. В `Evaluate()` читай/пиши `settler.Data` и вызывай capability-интерфейсы; верни `BTNodeState`.
4. Добавь узел в нужный `Behavior_`/`Job_` `Sequence`.
5. Экшены-передвижения возвращают `Running`, пока идут.

## Как добавить Condition

Либо оберни `Func<bool>` в инлайн-условие (`.While(...)` / `Conditional`), либо добавь leaf-`BTNode` в `AI/Node/Conditions` (пример — `IsTacticalMode`).

## Как добавить Job (два механизма!)

- **Реестровый** (через команды игрока): добавь значение в `JobType [Flags]` (`AI/Node/Jobs/Jobs.cs`), `CommandTarget.AddCapability`, регистрация в `ICommandService`, добавь `Job_X` в селектор-агрегатор джоб. Полный end-to-end — в `takeroot-commands`.
- **Сервисный** (без участия игрока): `Job_X : Sequence`, берущий цели из доменного сервиса, добавь напрямую в `BTRoot_Settler`.
- На провал/завершение чисть состояние: `ResetJobOnSettler` / `CompleteJob`.

## Приоритет = порядок добавления

`BTRoot_Settler` добавляет: сначала tactical, потом Behaviors нужд (`Death` > `CriticalTired` > `Electricity` > `Water` > `Energy` > `Care`), потом Jobs, потом Idle. Вставляй новое поведение на правильную позицию по приоритету.

## Capability-компоненты

Исполнение делегируется компонентам-`IPerformerComponent` (`IMovable`, `IBuilder`, `ICrafter`, `ICareGiver`, `IWaterer`, `IAttacker`, `ISearcher`...), которые резолвятся через `GetComponent` и `Init(WorkerAnimator)` в `Settler.SelfInit`.
- ⚠️ Префаб без любого из ожидаемых интерфейсов → **NRE** (каждый `Init` безусловный). Добавляя новый capability-интерфейс в `Settler`, проверь, что он есть на всех префабах поселенцев.
- ⚠️ `SimpleAttacker.Cancel` бросает `NotImplementedException`.

Детали capability/`WorkerComponent` — в `takeroot-commands`.

## Сетевая авторитетность ИИ

- Поселенцы тикают на `IsOwner` (== host сегодня), зомби — `IsHost`.
- Реплицируй состояние ИИ **явным `ClientRpc`** — `SettlerData` сериализует только `curMovePos`. См. `takeroot-netcode`.

## Нужды (отдельное дерево)

Нужды — **отдельное** дерево `_stateBt` → `Action_HandleNeedsChange`, на кулдаун-таймере, гейтится пороговыми свойствами (`IsTired`, `LowCare`...). Меняют `Data.needs.Value` (ссылка пере-`new`-ается при десериализации). Дебаг-байпасы: `Settler.GlobalGodmode` / `Immortal`.

## Зомби

Маленький `Selector(Behavior_Patrol, Behavior_ChaseNearbySettler)`. `ZombieData` — plain `[Serializable]` (**не сетевой**), мувер на UniTask. ⚠️ В `Zombie.cs` есть `using UnityEditor;`/гизмо-код — риск при IL2CPP-стрипе (обернуть в `#if UNITY_EDITOR`).

## Дебаг

`AI/Editor/SettlerEditor` (рефлексия по `_root`/`_state`/`_children`). ⚠️ `BTDebug`-логирование по умолчанию закомментировано.
