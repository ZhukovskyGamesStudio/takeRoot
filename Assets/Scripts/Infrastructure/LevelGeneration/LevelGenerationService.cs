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
            AI.SettlerData data = (AI.SettlerData)settler.GetData();

            data.names.Name = NamesList[Random.Range(0, NamesList.Count)];
            data.needs.Hp = Random.Range(data.needs.MaxHp / 4, data.needs.MaxHp);
            data.needs.Care = Random.Range(data.needs.MaxCare / 4, data.needs.MaxCare);
            data.needs.Hunger = Random.Range(0, data.needs.MaxHunger * 3 / 4);
            data.needs.Stress = Random.Range(0, data.needs.MaxStress * 3 / 4);
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