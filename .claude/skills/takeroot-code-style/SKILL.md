---
name: takeroot-code-style
description: >-
  Домашний кодстайл Take Root, выведенный из реального кода: именование, форматирование,
  C#-фичи, Unity-конвенции, идиомы архитектуры (ServiceLocator/MVP), UniTask/UniRx, и
  организация ассетов (префабы, сцены, ScriptableObject-конфиги, спрайты, разделение
  Resources vs Configs). Используй при написании или правке ЛЮБОГО C#-файла, SO-конфига,
  префаба или ассета в этом репозитории. Triggers: код, стиль, naming, именование, формат,
  SerializeField, namespace, префаб, конфиг, ScriptableObject, ассеты, как назвать.
---

# Take Root — кодстайл и организация ассетов

> Выведено из ~571 `.cs`. Где конвенции конфликтуют — указано «правило vs дрейф». **Пиши по правилу, не копируй дрейф.** Проект — долгострой; порядок и читаемость важнее всего.

## Именование

**Типы — PascalCase, с суффиксом роли** (сильная и консистентная конвенция):
- `*Service` (`FarmingService`), `*View` (`AvatarsView`), `*Presenter` (`PanelsPresenter`), `*Config` (`FarmingPlantConfig`), `*Data` (`SettlerData`), `*Manager` (только legacy + `AdminManager`), DI-регистраторы `*Loader_*` (`ServiceLocatorLoader_Main`).
- **Семейства через подчёркивание** для BT/суб-данных: `Behavior_Water`, `Action_Build`, `Job_Craft`, `BTRoot_Settler`, `Settler_Needs`, `Settler_EnergyData`. Файл называется как тип.
- BT-база с префиксом `BT`: `BTNode`, `BTNodeState`, `BTDebug`.

**Интерфейсы** — `I` + PascalCase. Сервисные расширяют маркер `IService` (`IFarmingService : IService`). Capability-интерфейсы поселенца — существительные-роли: `IMovable`, `IBuilder`, `ICrafter`, `IWaterer`, `IAttacker`, `IResourceCarrier`.

**Поля:**
- Приватные инстанс-поля — **`_camelCase`** (правило, ~869 примеров: `_assetProvider`, `_root`, `_stateBt`). Голый `camelCase` без `_` встречается в старых data-классах (`currJob`, `curMovePos`) — это дрейф, в новом коде пиши `_camelCase`.
- `[SerializeField]` — **всегда на отдельной строке** над приватным `_field` (никогда `[SerializeField] private` в одну строку). Несколько полей одного типа можно в одну строку: `private SpriteRenderer _plotView, _plantView;`.
- Сериализуемые авто-свойства (особенно в SO) — `[field: SerializeField] public T Prop { get; set; }`.
- `[HideInInspector]` — чтобы выставить public рантайм-поле без показа в инспекторе.
- ⚠️ Анти-паттерн: `[SerializeField] public PascalCase` (встречается в `FarmingPlot`) — не повторять, предпочитай `private [SerializeField] _field`.

**Свойства** — PascalCase. Вычисляемые — expression-bodied get-only: `public SettlerData Data => NetworkData.Value;`, `public bool HasJob => currJob != JobType.None;`. Хранимое состояние — `{ get; private set; }`.

**Методы** — PascalCase-глаголы. Обработчики событий — префикс `On*` (`OnSelectionEnd`). Сетевые методы строго по правилам Netcode: `*ServerRpc` / `*ClientRpc` парами (`ChangePlantServerRpc` → `SyncPlantClientRpc`).

**Константы** — непоследовательно (PascalCase / `ALL_CAPS` / camelCase). **Дефолт — PascalCase.**

## Namespace

Namespace объявлены лишь в ~25% файлов — это **дрейф, не замысел**. Большинство файлов без namespace (глобальный): все UI View/Presenter, сервисы, `*Manager`, `EntryPoints`. Где есть — зеркалят папки только в поддереве `AI.*` (`AI`, `AI.Node`, `AI.Node.Jobs`). `ServiceLocator` живёт в `CodeBase.Services` (наследие туториала). Единого корневого namespace нет.

> Для нового общего/переиспользуемого кода **предпочитай namespace по папке** (улучшаем консистентность), но не ломай существующий глобальный код массовым переносом.

## Форматирование

- **K&R / египетские скобки** — твёрдое правило: `{` на той же строке, что и объявление. Блоки **всегда в скобках**, даже однострочные `if`.
- **Отступ — 4 пробела.** ~66 старых файлов (в `AI/Node/Actions`, `AI/Behaviors`, `PanelsPresenter`) на табах — это дрейф; новый код пиши пробелами.
- `else` обычно на своей строке после `}` (`} else {`).
- **Guard-clause / ранний return** — предпочитаемый стиль (`if (!inProgress) { return; }`).
- Длинные списки параметров/`new(...)` переносятся на строки-продолжения с отступом.
- Пустые строки разделяют логические группы. **`#region` не используется (0 раз) — не вводить.**

## C#-фичи

- `var` — свободно, где тип очевиден; явные типы тоже норм (оба сосуществуют, нет «var везде»).
- Expression-bodied члены — очень часто (`SetMood(Mood m) => WorkerAnimator.SetMood(m);`).
- Target-typed `new()` — активно (`List<...> x = new();`).
- Pattern matching (`is`), switch-выражения, generic-ограничения (`where T : IService`), интерполяция строк `$"..."`, null-conditional/coalescing (`?.`, `??=`).
- **Nullable reference types НЕ включены. Records НЕ используются** — данные это plain `[Serializable]`-классы.

## Unity-конвенции

- Инспектор-данные — приватный `[SerializeField] _field`, не public.
- `[Header("...")]` для группировки в крупных data-классах (`SettlerData`). `[Tooltip]` не используется. `[RequireComponent(typeof(X))]` — где есть зависимость от соседнего компонента.
- SO-конфиги: `[CreateAssetMenu(fileName="X", menuName="Scriptable Objects/X", order=0)]` + `[field: SerializeField]` авто-свойства.
- Жизненный цикл: `Awake()` — присвоение синглтонов в entry points; `Start()` — инициализация (часто под `IsHost`); `Update()` — пер-кадр; `NetworkBehaviour` — `OnNetworkPostSpawn()` для сетевой инициализации, `base.*` вызывать первым.
- `GetComponent` **кэшировать** в `Init`/`SelfInit`, не пер-кадр. Использовать `TryGetComponent(out X)`.
- Поиск объектов — **`FindObjectsByType<T>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)`** (не устаревший `FindObjectsOfType`). ⚠️ Уже злоупотребляют пер-кадровыми сканами — **новые пер-кадровые `FindObjectsByType` не добавлять**.

## Идиомы архитектуры (следовать)

- **ServiceLocator** — центральный DI. `ServiceLocator.Container.RegisterSingle<T>(impl)` / `.Single<T>()`, всё реализует `IService`. Детали — `takeroot-bootstrap-and-di`.
- **Композиционный корень** — ручной constructor injection в `*Loader`-классе (`ServiceLocatorLoader_Main`).
- Два стиля доступа к зависимостям: (1) чистый ctor-injection в plain-классы (предпочтительно для не-MonoBehaviour); (2) прямой `ServiceLocator.Container.Single<T>()` внутри MonoBehaviour/RPC, где ctor-инъекция невозможна.
- **MVP для UI:** `View` = пассивный MonoBehaviour с `[SerializeField]`-виджетами и `SetData/Init`; `Presenter` = plain-класс, создаётся в `CoreEntryPoint.InitPresenters()`, реализует `IDisposable` (часто + `IUpdatable`). Детали — `takeroot-ui-mvp`.
- **Кастомный update-пул:** реализуй `IUpdatable`, `Register` в конструкторе, `Unregister` в `Dispose`.
- **Async:** UniTask через `IAsyncRunner` (для нового кода предпочтительнее корутин). **Реактивность:** UniRx `ReactiveProperty<T>` + `.Subscribe(...)`. ⚠️ Подписки часто не диспозят (`AddTo`/`CompositeDisposable`) — это утечка; **новые подписки трекай и диспозь**.

## Организация файлов

- Один публичный тип на файл; имя файла == имя типа.
- Маленькие спутники-enum'ы часто в конце файла-владельца (`BTNodeState` в `Node.cs`, `DeathCause` в `Settler.cs`) — норм.
- Интерфейс и реализация — отдельные файлы в одной папке (`IFarmingService.cs` + `FarmingService.cs`).
- Порядок `using`: `System.*` → сторонние (`CodeBase`, `UniRx`, `Unity.Netcode`, `Cysharp`) → `UnityEngine.*`; алиас `using Object = UnityEngine.Object;` для дизамбигуации.

## Комментарии

Плотность низкая-умеренная, преимущественно английский. **XML-доков (`///`) нет** — не вводить массово. Инлайн `//`, `//TODO`. Двуязычно: ~34 файла с русскими комментариями (Гоша иногда пишет заметки по-русски). Комментарии объясняют **зачем/TODO**, а не пересказывают код. ⚠️ Большие закомментированные блоки мёртвого кода в репо есть — **не повторять**, не оставлять.

## Анти-паттерны (не тиражировать)

Табы вместо 4 пробелов; закомментированный мёртвый код; `[SerializeField] public` поля; несогласованный регистр констант; пустые тела/`NotImplementedException`-заглушки; нетрекаемые `.Subscribe`; новые `*Command`-подклассы (`BaseCommand` `[Obsolete]` — иди через `BTNode`); пробелы/опечатки/`z_`-префиксы в именах файлов.

---

# Организация ассетов

## Префабы — `Assets/Prefabs/`

PascalCase-файлы в PascalCase-папках по доменам: `Building`, `Creatures`, `Decor`, `Farming`, `Furniture`, `Menu`, `Networking`, `Projectiles`, `Quests`, `Resources`, `Structures`, `UI`. Примеры: `Building/Biogenerator.prefab`, `Creatures/ChamomileCharacter.prefab`, `Furniture/WoodenDoor.prefab`. Без глобального префикса/суффикса типа.

> ⚠️ Не повторять реальные косяки: пробелы в именах (`Craft Recipe.prefab`, `Sofa 1.prefab`), опечатки (`Bookshhelf`), `z_`-префикс для сортировки (`z_CraftingTable`), стейджинг-папки `NEW PREFABS`/`Test`. UI-вью ресурсов — суффикс `UI`/`Ui` (`HammerUI.prefab`), приводи к единому `UI`.
> `Prefabs/Resources` — обычная доменная папка для префабов-ресурсов, **не** спец-папка `Assets/Resources`. Не путать.

## ScriptableObject-конфиги — РАЗДЕЛЕНИЕ ХРАНЕНИЯ ⚠️

Классы SO лежат рядом с кодом фичи (`Scripts/Building/BuildingsConfig.cs`). Меню — **`Scriptable Objects/<TypeName>`** (несколько выбиваются: `World Config`, `Quests/QuestConfig`, `GameResources/ResourcesConfig` — для нового следуй общему паттерну).

**Где хранить `.asset` — зависит от способа загрузки:**
- Грузится в рантайме через `Resources.Load(All)` → **`Assets/Resources/Configs/...`** по **точному** подпути из кода: `Configs/FarmingPlants`, `Configs/BuildingRecipes` (+`HumanRecipes`), `Configs/Occurencies`; синглтоны прямо в `Configs` (`BuildingsConfig`, `ResearchConfig`, `ResourcesTable`, `WorldConfig`, `TimeScaleConfig` и т.д.).
- Ссылается по **GUID** (не по пути) → **`Assets/Configs/...`**: `CraftingRecipeConfig` в `Configs/Crafting Recipes/<Station>/`, `MainInfoData` в `Configs/MainInfoConfigs/...`.

> Положишь path-loaded конфиг не туда — он **молча не загрузится**. Это частая ошибка; сверяйся с реальной строкой пути в `ConfigsProvider.cs`.

## Прочие ассеты

- **Сцены** `Assets/Scenes/` — PascalCase + суффикс `Scene` (`CoreScene.unity`); тесты в `Tests/`, катсцены в `Videos/`.
- **Спрайты** `Assets/Sprites/` — **snake_case** PNG с префиксом субъекта (`lamp_idle.png`, `bottle_biofuel.png`, `floor_0.png`, `common_button.png`), глубоко вложены по доменам. Стейджинг-папки `NEW ART TO BE ORGANIZED`, `TO DELETE`, `Ref`/`Refs` — временные, готовое раскладывай по доменным папкам.
- **Анимации** `Assets/Animations/` — PascalCase. Поселенцы: база `ActionsController.controller` + per-character `Actions<Char>.overrideController`; базовые клипы — глаголы (`Idle`, `Move`), персональные — `<Verb><Char>` (`IdleLamp`).
- **Шрифты** `Assets/Fonts/` — `.ttf` + сгенерированный `<Family> SDF.asset` (регенерировать SDF при смене шрифта).
- **`.meta`-файлы** несут стабильный GUID — **никогда не удаляй/переименовывай их отдельно**, иначе ломаются ссылки.
- Loose-ассеты в корне `Assets/` (`DefaultNetworkPrefabs.asset`, `DefaultVolumeProfile.asset`, URP-настройки) — Unity-managed, не двигать.
