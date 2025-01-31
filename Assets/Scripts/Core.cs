using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour {
    public static Core Instance;
    public static CoreCanvasUi UI;
    public static CommandsManagersHolder CommandsManagersHolder;
    public static SettlersManager SettlersManager;

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
            return UI.NetworkReplacement.CurrentRace;
        }
    }
}