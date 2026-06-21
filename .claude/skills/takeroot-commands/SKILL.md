---
name: takeroot-commands
description: >-
  Система команд Take Root: как клик/назначение игрока превращается в работу поселенца —
  живой стек JobType + CommandTarget + capability WorkerComponents, исполняемый через дерево
  поведения; поток input→ServerRpc→реестр ICommandService→BT→capability. Используй при
  добавлении/правке команды игрока: новый JobType, CommandTarget capability, TargetComponent-
  эффект, worker/capability-компонент. Triggers: command, команда, JobType, CommandTarget,
  ICommandService, capability, WorkerComponent, TargetComponent, Action_FindJob, назначить
  работу, клик игрока, designation.
---

# Take Root — система команд

> Сначала `takeroot-project-context`. ⚠️ Двухголовая система: **строй на `JobType`/`CommandTarget`**, а **НЕ** на `BaseCommand`/`ConcreteCommands`/`Worker`/`WorkerAssigner`/`CommandType` — всё это `[Obsolete]`/мёртвое. `JobType` (`AI/Node/Jobs/Jobs.cs`) живой; `CommandType` — мёртв и с другим бит-лейаутом.

## Поток

```
клик/хоткей игрока
  → JobCommandsInputHandlerService / CommandView
  → CommandTarget.TrySetJobServerRpc(RequireOwnership=false)        [хост]
  → ICommandService реестр джоб (host-authoritative)
  → дерево поведения поселенца: Action_FindJob берёт джобу
  → Job_X (Sequence): Action_MoveTo + Action_<эффект>
  → capability WorkerComponent выполняет; эффект-состояние реплицируется своими RPC
```

Исполняющая половина живёт в **другой папке** — `AI/Node/Jobs` (см. `takeroot-ai-behavior-trees`).

## End-to-end: добавить команду

1. **JobType** — добавь значение в `[Flags] enum JobType` (`AI/Node/Jobs/Jobs.cs`).
2. **TargetComponent** — в `Commands/.../TargetComponents` создай компонент со state + методами-эффектами; обычно `NetworkBehaviour` с `ServerRpc`/`ClientRpc` для реплицируемого состояния эффекта.
3. **CommandTarget.Start()** — `TryGetComponent` + `AddCapability(JobType.X)`; выстави методы-эффекты и предикат «готово».
4. **Capability-интерфейс** — `IX : IPerformerComponent { Init(WorkerAnimator); Cancel(); }` + реализация-`WorkerComponent` (UniTask perform-цикл + хук `WorkerAnimator` + `Cancel` через `CancellationTokenSource`).
5. **Settler** — добавь поле интерфейса в `AI.Settler` и проводку `GetComponent` + `Init` в `SelfInit`.
6. **Job_X : Sequence** — `Action_MoveTo` + новый `Action`; зарегистрируй в селекторе `Jobs.cs`.
7. **Триггер игрока** — хоткей в `JobCommandsInputHandlerService` и/или toggle в `CommandView`.

## Матчинг capability (битмаска, на стороне цели)

- Подходит, если `(JobCapabilities & job) == job`.
- Одна джоба на цель — `Data.HasJob`.
- Истина резервации — `Data.AssignedSettler != null` (⚠️ игнорируй legacy-bool `Reserved`).
- `CommandTargetData.Id` должен быть **уникален** для словаря `JobTargets`.

## Сеть / авторитетность

- `TrySetJobServerRpc` / `CancelJobServerRpc` (`RequireOwnership=false`) принимают джобы; `ChangePlannedJobClientRpc` зеркалит визуал.
- Состояние эффекта реплицируется **собственными RPC компонента**.
- BT оценивают только owner-поселенцы; реестр остаётся host-authoritative.
- `Race` передаётся явно и перепроверяется в `Jobs.JobCondition`.
- Подробности сетевых правил — `takeroot-netcode`.

## Реестр и назначение

- `ICommandService.RegisterJob` / `GetJob` возвращает **первую** не-Reserved запись (нет приоритета/сортировки по дистанции — `//TODO`).
- `Action_FindJob` клеймит джобу: ставит `Data.AssignedSettler` + `settler.currJob`/`currTarget`.
- `ResetJobOnSettler`/`CompleteJob` обязаны очистить **всё**: `Data.Reserved`/`AssignedSettler`/`CurrentJobId`/`currJob`.

## Сетевой сплит компонентов

- `NetworkBehaviour` (реплицируют через `ClientRpc`): `Mover`, `Searcher`, `WorkerAnimator`.
- Plain `MonoBehaviour` (эффекты идут через RPC `CommandTarget`): `Destroyer`, `Waterer`, `Researcher`, `SimpleAttacker`.

## Известные опасные места

> ⚠️ требует проверки у Гоши.

- ⚠️ `SimpleAttacker.Cancel` бросает `NotImplementedException`.
- ⚠️ Отсутствие capability-компонента → NRE в `Settler.SelfInit`.
- `WorkerAssigner` мёртв (`Update` закомментирован, `_selection` не присваивается) — не вызывать `FindAvailableWorker`.
- `CommandService.HandleCommandRequest` — пустая obsolete-заглушка.
- `ServiceLocator.Single<ICommandService>()` дёргается инлайн в RPC (некэшировано, `//TODO`).
