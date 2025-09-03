using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelGenerationService : ILevelGenerationService {
    public async UniTask Generate() {
        GenerateSettlers();
    }

    private void GenerateSettlers() {
        List<SettlerSelectable> settlers = Object.FindObjectsByType<SettlerSelectable>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .ToList();
        foreach (SettlerSelectable settler in settlers) {
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