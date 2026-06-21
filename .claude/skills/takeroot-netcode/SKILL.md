---
name: takeroot-netcode
description: >-
  Сетевая модель Take Root (Netcode for GameObjects): строго host-authoritative,
  идентичность игрока через Race (а не ownership), канонический паттерн ServerRpc→ClientRpc,
  ловушка сериализации NetworkVariable<SettlerData> (только curMovePos), NetworkDataHolder,
  spawn/despawn, лобби-флоу, мёртвый Steam-транспорт, AdminManager.IsFakeOnline. Используй
  при любом сетевом коде: RPC, NetworkVariable, spawn/despawn, репликация состояния,
  отладка десинков и вопросов «кто это выполняет». Triggers: network, netcode, RPC,
  ServerRpc, ClientRpc, NetworkVariable, NetworkBehaviour, IsHost, IsOwner, spawn,
  NetworkDataHolder, Race, десинк, сеть, репликация, host.
---

# Take Root — сеть (NGO, host-authoritative)

> Сначала `takeroot-project-context`. Живой транспорт — `UnityTransport` (прямой IP). `Steam/`-транспорт мёртв.

## Топология

Строго **host-authoritative**. Два игрока: **Host** (+играет) и **Client**.
- Клиент **только** шлёт `[ServerRpc(RequireOwnership=false)]` и получает `[ClientRpc]`-эхо.
- Host-only работу гейти на `INetworkService.IsHost` (он **OR'ится** с `AdminManager.IsFakeOnline` — учитывай в одиночной отладке).

## Идентичность игрока = Race

Идентичность — **`Race`** (`Plants`/`Robots`), хранится в `NetworkVariable`-ах `MainGameNetworkData` на `NetworkDataHolder`. **Никогда `OwnerClientId`.**
- Используй `MyRace()` / `INetworkService.MyRace`.
- Host = условно клиент 1, присоединившийся = клиент 2.

## Канонический паттерн RPC

```csharp
// публичная обёртка → ServerRpc (мутация на хосте) → парный ClientRpc (общий apply)
public void DoThing(Race race) => DoThingServerRpc(race);

[ServerRpc(RequireOwnership = false)]
private void DoThingServerRpc(Race race) {
    // ... мутируем авторитетное состояние ...
    DoThingClientRpc(race);          // рассылаем всем
}

[ClientRpc]
private void DoThingClientRpc(Race race) {
    // общий apply-метод; ВЫПОЛНЯЕТСЯ И НА ХОСТЕ ТОЖЕ
}
```

> ⚠️ Хост **двойно применяет** состояние из собственного `ClientRpc`. Поэтому пиши **идемпотентные мутации абсолютных значений**, а не аддитивные (`x = value`, не `x += delta`). `Race` передавай явным аргументом RPC.

## ⚠️ Ловушка сериализации SettlerData

`INetworkSerializable.NetworkSerialize` у `SettlerData` шлёт **только `curMovePos`**. Всё остальное реплицируемое состояние поселенца требует **явного `ClientRpc`** (паттерны `UpdateNeedsClientRpc` / `UpdateNamesDataClientRpc` / `SetTacticalClientRpc`), а не авто-синка `NetworkVariable`.
- Ссылки на объекты **никогда** не реплицируются.
- `Equals`/`GetHashCode` покрывают несериализуемые поля (детект «грязного» состояния расцеплён с реальным синком).

При добавлении нового сетевого поля поселенца — заведи под него отдельный `ClientRpc`. См. `takeroot-ai-behavior-trees`.

## Реальность ownership

`ChangeOwnership`/`SpawnWithOwnership` **нигде не вызывается**. Поселенцы — пред-размещённые scene `NetworkObject`, поэтому `IsOwner == host` для всех → **хост гоняет все деревья поведения поселенцев**.
- Тик поселенца гейтится на `(IsOwner || IsFakeOnline)`; зомби тикают **host-only**.
- ⚠️ Долгосрочная модель владения — открытый вопрос к Гоше (давать ли клиенту реальный ownership своей расы). До ответа новый ИИ-код считай host-исполняемым.

## Spawn / despawn

- Только на хосте: `INetworkService.InstantiateAndSpawn<T>` и `NetworkObject.Despawn` (⚠️ внутреннего гарда нет — гейти вызовы на `IsHost` сам).
- Очистка — в `OnNetworkDespawn` → `Destroy`.
- Сетевая инициализация — в `OnNetworkPostSpawn`.
- Перед обращением к `NetworkDataHolder` гарди через `NetworkDataHolder.IsCreated` / `OnCreated`.

## NetworkDataHolder (`Online/NetworkDataHolder.cs`)

`DontDestroyOnLoad`-синглтон, агрегирует `MainGameNetworkData` / `SelectRaceNetworkData` / `ResearchNetworkData`.
- ⚠️ Часть `NetworkVariable` объявлена **без инициализатора** (`HostRace`/`ClientRace`/`HostReady`/`ClientReady`) → NRE-ловушка, должны быть проставлены через инспектор; `ResearchNetworkData` использует `= new()`.
- ⚠️ Возможен **двойной спавн**: одни пути гардят `FindAnyObjectByType`, другие нет.
- `Instance` должен быть заспавнен **до** RPC времени/исследований (иначе NRE).

## Транспорт и лобби

- **Живое:** `UnityTransport` поверх LAN IP (`IpConnection` в `Assets/TestPrototype/Scripts`). Обработки ошибок/таймаутов при джойне нет.
- **Мёртвое:** `SteamNetworkTransport` (legacy P2P, AppId-заглушка 480) — **не предполагай**, что Steam P2P/лобби работают.
- Лобби-флоу: `StartHost` + спавн `NetworkDataHolder` + named-message `OpenChooseRace` + пер-кадровый поллинг ready-флагов → host-only `SceneManager.LoadScene(CoreScene)`.
- ⚠️ Не копируй `PrepareGamePanel.SpawnDataHolder`: `[ServerRpc]` на не-`NetworkBehaviour` бессмысленен (выполнится локально).

## AdminManager.IsFakeOnline

Одиночный шорткат: поднимает host-сессию, фейкает ready, назначает `HostRace=Plants`/`ClientRace=Robots`, **OR'ится в `IsHost`/`IsOwner`**. Весь сетевой код обязан корректно работать при включённом `IsFakeOnline`.
