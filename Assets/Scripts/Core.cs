using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour , IResetable {
    public static Core Instance;
    public static CoreCanvasUi UI;
    public static CommandsManagersHolder CommandsManagersHolder;
    public static SettlersManager SettlersManager;
    public static GridManager GridManager;
    public static ConfigManager ConfigManager;
    public static FogOfWarManager FogOfWarManager;
    public static CraftingManager CraftingManager;
    public static BuildingManager BuildingManager;
    public static AStarPathfinding AStarPathfinding;
    public static SelectionManager SelectionManager;
    public static SettlersSelectionManager SettlersSelectionManager;
    public static GameEventsManager GameEventsManager;
    public static ResourceManager ResourceManager;
    public static QuestManager QuestManager;
    public static WateringManager WateringManager;
    
    public Race CurrentNetworkFakeRace = Race.Plants;

    private void Awake() {
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
        if (NetworkManager.Singleton != null) {
            return PlayerRaceSelection.GetRace();
        } else {
            return CurrentNetworkFakeRace;
        }
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