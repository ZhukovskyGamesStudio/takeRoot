using System.Collections.Generic;
using Settlers.Crafting;
using UnityEngine;

public class CraftingStationableData : ECSComponent
{
    [field: SerializeField]
    public List<CraftingRecipeConfig> AvailableRecipes { get; private set; }
    [field: SerializeField]
    public string Name {get; private set;}
    [field: SerializeField]
    public Sprite InfoBookIcon {get; private set;}
    

    public override int GetDependancyPriority()
    {
        return 0;
    }

    public override void Init(ECSEntity entity)
    {
    }
    
}