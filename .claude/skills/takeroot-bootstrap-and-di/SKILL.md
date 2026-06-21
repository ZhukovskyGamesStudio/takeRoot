---
name: takeroot-bootstrap-and-di
description: >-
  Бутстрап и DI Take Root: boot-цепочка сцен, кастомный ServiceLocator (IService),
  композиционный корень ServiceLocatorLoader_Main, добавление/порядок сервисов,
  тик-цикл IUpdateService/IUpdatable, UniTask через IAsyncRunner, загрузка конфигов
  ConfigsProvider/AssetProvider, GameFactory, идентификаторы. Используй при
  добавлении/правке сервиса, работе с boot-флоу, update-циклом, загрузкой ресурсов,
  и при отладке null-сервисов или стейл-синглтонов. Triggers: ServiceLocator,
  IService, RegisterSingle, Single, EntryPoint, CoreEntryPoint, composition root,
  сервис, DI, IUpdatable, IAsyncRunner, ConfigsProvider, GameFactory, бутстрап.
---

# Take Root — бутстрап и DI

> Сначала прочти `takeroot-project-context` (live vs legacy). Здесь — живой стек. Legacy-бутстрап (`ObsoleteCoreEntryPoint` + `IInitableInstance` + `Managers/`) **не использовать**.

## Boot-флоу

```
EntryPointBase (NetworkBehaviour) — корень каждой сцены
  Awake() → TrySwitchToLoading(): однократно грузит LoadingScene (гард LoadingEntryPoint.LoadingSceneVisited)
  ⚠️ при ПЕРВОМ запуске Awake аборится (return) до любой проводки
LoadingEntryPoint.Start → грузит MenuScene
MenuEntryPoint.Play → грузит CoreScene ТОЛЬКО если NetworkManager.Singleton.IsHost
  (клиент переходит через сетевую загрузку сцены от хоста, не по кнопке)
CoreEntryPoint.Awake  =  РЕАЛЬНЫЙ композиционный корень:
  1. строит ServiceLocatorLoader_Main(...передаёт scene-рефы...)
  2. RegisterServices()
  3. заполняет DataProvider (new WorldResourcesData / CreaturesData)  ← ⚠️ ordering: после RegisterServices
  4. InitPresenters()  → MVP-презентеры (см. takeroot-ui-mvp)
CoreEntryPoint.OnNetworkPostSpawn → GenerateLevel().Forget() ТОЛЬКО при IsHost
```

Ключевые файлы: `EntryPoints/EntryPointBase.cs`, `Infrastructure/CoreEntryPoint.cs`.

## ServiceLocator

`Infrastructure/ServiceLocator/ServiceLocator.cs` (namespace `CodeBase.Services`):
- Статический синглтон `ServiceLocator.Container`.
- `RegisterSingle<TService>(impl) where TService : IService` и `Single<TService>()`.
- Хранилище — трюк со статиком вложенного generic-класса `Implementation<TService>.ServiceInstance` (один слот на закрытый тип).
- Все сервисы реализуют маркер `IService`.

> ⚠️ **Контейнер глобальный и НИКОГДА не сбрасывается** между сессиями/перезагрузкой сцены. При возврате в меню и повторной игре старые инстансы сервисов переживают. Учитывай при ре-хосте и `FastPlaymode`/`IResetable` (которые сбрасывают только legacy-статику).

## Как добавить сервис

1. Заведи `IXxx : IService` + реализацию `Xxx` (отдельные файлы в одной папке).
2. В `ServiceLocatorLoader_Main.RegisterServices()` добавь:
   ```csharp
   _services.RegisterSingle<IXxx>(new Xxx(_services.Single<IDep>(), ...));
   ```
   **строго ПОСЛЕ** регистрации всех зависимостей (`_services.Single<IDep>()` резолвит уже зарегистрированное).
3. Если сервису нужны scene-рефы — прокинь их в конструктор `ServiceLocatorLoader_Main` из `CoreEntryPoint`.
4. Резолв в рантайме: `ServiceLocator.Container.Single<IXxx>()` (или ctor-инъекция в plain-классах).

> Порядок регистрации **критичен и не валидируется**: нет проверки циклов, last-write-wins. Переставишь строки — тихо получишь null-зависимость.

## Факты и опасности регистрации (`ServiceLocatorLoader_Main`)

- `IPathfindService` регистрируется **дважды**: сначала `MockPathfindService`, затем (после `CreateMap`/`CreateSimpleGraph`) перезаписывается на `AStar`. Резолв до перезаписи даст mock. См. `takeroot-grid-pathfinding`.
- `IWorldReader` и `IWorldWriter` указывают на **один** инстанс `WorldState` (намеренное разделение чтение/запись).
- Карта (`MapFromSceneObjects.CreateMap`) строится **до** `AStar`/`GridService`.
- ⚠️ **`IGameFactory` НЕ регистрируется**, хотя потребляется (`ResourceSpawnPoint` и `TacticalMergable` — последний в legacy ECS `WorldObjects/EcsSystem/Tactical`). Оба потребителя и сам `GameFactory` не дорабатывались (только mass-format 2025-08-26) → это **недописанный/заброшенный код**, а не активный краш. Будешь оживлять потребителя — сначала добавь регистрацию `IGameFactory` в `RegisterServices`.

## Тик-цикл

`Infrastructure/Update/` — `IUpdateService` + `IUpdatable`:
- `UpdateService` — MonoBehaviour, его `Update()` (магический колбэк Unity) итерирует зарегистрированные `IUpdatable` (через копию `ToArray` → GC-горячо).
- Plain-сервис, которому нужен пер-кадровый тик: реализуй `IUpdatable`, `Register(this)` в конструкторе, `Unregister(this)` в `Dispose`. ⚠️ `Dispose` редко вызывается при teardown сцены — помни про утечки.
- `UpdateService` также реализует `ICoroutineRunner` (обёртка над `StartCoroutine`).

## Async / корутины

- `IAsyncRunner` / `UniTaskAsyncRunner` (`Infrastructure/Async/`): `Wait`, `WaitAndDo`, `WaitUntil`, `WaitUntilAndDo`.
- ⚠️ В обёртке `Wait`/`WaitAndDo` **игнорируют** переданный `CancellationToken` — только `WaitUntil` его форвардит. Проверяй, если завязываешься на отмену.
- Для нового кода — UniTask/`IAsyncRunner` предпочтительнее корутин.

## Загрузка конфигов и ассетов

- `ConfigsProvider` (`Infrastructure/ConfigsProvider.cs`): в конструкторе синхронно кэширует все SO через `Resources.LoadAll`. ⚠️ Использует `FirstOrDefault` — дубликат типа SO в `Configs/` даёт недетерминированный выбор. Это единая точка доступа ко всем спискам контента (см. `takeroot-gameplay-content`).
- `AssetProvider` (`Infrastructure/Assets/AssetProvider.cs`): `Resources.Load` + `Instantiate` (⚠️ без null-проверки). Пути — в `AssetPath`.
- **Не Addressables.** Разделение хранения SO (`Resources/Configs` vs `Assets/Configs`) — в `takeroot-code-style`.

## Factory и идентификаторы

- `GameFactory` (`Infrastructure/Factory/GameFactory.cs`): `CreateSettler`/`CreateResource`. ⚠️ Аудит отметил баги (пустое тело инициализации `Worker`, `CreateCommand` бросает `NotImplementedException`, часть ctor-зависимостей игнорируется) — проверь перед использованием.
- `IdentifierService` (`IIdentifierService`): ⚠️ id **локальные, не сетевые** — `Next()` нельзя использовать для кросс-пир идентичности (см. `takeroot-netcode`).
