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
            
            data.needs.Value.Hp = Random.Range(data.needs.Value.MaxHp / 4f, data.needs.Value.MaxHp);
            data.needs.Value.CareData.currentCare = Random.Range(data.needs.Value.CareData.maxCare / 2f, data.needs.Value.CareData.maxCare);
            data.needs.Value.SatietyData.currentSatiety = Random.Range(data.needs.Value.SatietyData.maxSatiety / 2f, data.needs.Value.SatietyData.maxSatiety);
            data.needs.Value.StressData.currentStress = Random.Range(0, data.needs.Value.StressData.maxStress * 3f / 4);
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