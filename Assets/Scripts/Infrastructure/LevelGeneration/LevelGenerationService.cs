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
        List<SettlerSelectable> settlers = Object.FindObjectsByType<SettlerSelectable>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .ToList();
        foreach (SettlerSelectable settler in settlers) {
            Random.InitState((int)seed + settler.GetInstanceID());
            AI.SettlerData data = settler.GetComponent<AI.Settler>().Data;

            data.names.Name = NamesList[Random.Range(0, NamesList.Count)];
            data.needs.Hp = Random.Range(data.needs.MaxHp / 4, data.needs.MaxHp);
            data.needs.CareData.currentCare = Random.Range(data.needs.CareData.maxCare / 2, data.needs.CareData.maxCare);
            data.needs.SatietyData.currentSatiety = Random.Range(data.needs.SatietyData.maxSatiety / 2, data.needs.SatietyData.maxSatiety);
            data.needs.StressData.currentStress = Random.Range(0, data.needs.StressData.maxStress * 3 / 4);
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