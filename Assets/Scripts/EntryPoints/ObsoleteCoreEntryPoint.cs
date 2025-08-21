using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
[Obsolete]
public class ObsoleteCoreEntryPoint : EntryPointBase, IResetable {
    public static ObsoleteCoreEntryPoint Instance;
    public static CoreCanvasUi UI;
    public static CommandsManagersHolder CommandsManagersHolder;
    public static SettlersManager SettlersManager;
    public static GridManager GridManager;
    public static ConfigManager ConfigManager;
    public static FogOfWarManager FogOfWarManager;
    public static CraftingManager CraftingManager;
    public static BuildingManager BuildingManager;
    public static AStarPathfinding AStarPathfinding;
    public static AStarPathfindingVertical AStarPathfindingVertical;
    public static SelectionManager SelectionManager;
    public static SettlersSelectionManager SettlersSelectionManager;
    public static GameEventsManager GameEventsManager;
    public static ResourceManager ResourceManager;
    public static QuestManager QuestManager;
    public static WateringManager WateringManager;
    public static PowerManager PowerManager;
    public static LayerManager LayerManager;
    public static WarpManager WarpManager;
    

    public Race CurrentNetworkFakeRace = Race.Plants;

    public Action<Race> OnChangeRace;

    private void Awake() {
        if (TrySwitchToLoading()) {
            return;
        }
        
        Instance = this;

        LoadAndInit();
    }

    private void LoadAndInit() {
        List<IInitableInstance> instances = new();

        foreach (MonoBehaviour monoBehaviour in FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
            if (monoBehaviour is IInitableInstance initable) {
                instances.Add(initable);
            }
        }

        Queue<IInitableInstance> initables = new(instances);

        List<Type> initedList = new();
        int iterationLimit = 1000;

        for (int i = 0; i < iterationLimit; i++) {
            if (initables.Count == 0) {
                return;
            }

            IInitableInstance next = initables.Dequeue();
            if (!next.GetDependencies().Except(initedList).Any()) {
                next.Init();
                initedList.Add(next.GetType());
            } else {
                initables.Enqueue(next);
            }
        }

        throw new StackOverflowException($"Probably cyclic dependencies {JsonUtility.ToJson(initables)}");
    }

    public void GoToMenu() {
        SceneManager.LoadScene("MenuScene");
    }

    public Race MyRace() {
        return NetworkManager.Singleton != null ? PlayerRaceSelection.GetRace() : CurrentNetworkFakeRace;
    }

    public void SwitchFakeRace() {
        CurrentNetworkFakeRace = CurrentNetworkFakeRace switch {
            Race.Plants => Race.Robots,
            Race.Robots => Race.Plants,
            _ => CurrentNetworkFakeRace
        };
        OnChangeRace?.Invoke(CurrentNetworkFakeRace);
    }

    public void Reset() {
        UI = null;
        CommandsManagersHolder = null;
        SettlersManager = null;
        GridManager = null;
        ConfigManager = null;
        FogOfWarManager = null;
        CraftingManager = null;
        BuildingManager = null;
        AStarPathfinding = null;
        SelectionManager = null;
        SettlersSelectionManager = null;
        GameEventsManager = null;
        ResourceManager = null;
        QuestManager = null;
    }
}