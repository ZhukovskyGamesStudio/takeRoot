using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerationService : ILevelGenerationService {
    private readonly INetworkService _networkService;

    public LevelGenerationService(INetworkService networkService) {
        _networkService = networkService;
    }

    public async UniTask Generate() {
        float seed = Random.Range(0, 100000);
        Debug.Log("seed" + seed);
        NetworkDataHolder.Instance.ClearAndCombineTilemaps();
        NetworkDataHolder.Instance.GenerateRandomDecor(seed);
        //TODO refactor this with proper init waiting
        await UniTask.WaitForSeconds(0.5f);
        
        NetworkDataHolder.Instance.ClearAndCombineTilemapsClientRpc();
        NetworkDataHolder.Instance.GenerateRandomDecorClientRpc(seed);

        GenerateSettlers(seed);
    }

    private void GenerateSettlers(float seed) {
        List<AI.Settler> settlers = Object.FindObjectsByType<AI.Settler>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .ToList();
        foreach (AI.Settler settler in settlers) {
            Random.InitState((int)seed + settler.GetInstanceID());
            AI.SettlerData data = settler.Data;

            data.names.Name = NamesList[Random.Range(0, NamesList.Count)];
            settler.UpdateNamesDataClientRpc(data.names.Name);
        }
    }

    private static List<string> NamesList = new() {
        "Гоша",
        "Паша",
        "Дима",
        "Дима II",
        "Ира",
        "Вова",
        "Марина",
        "Алёна",
        "Дима III",
        "Кирилл"
    };
}