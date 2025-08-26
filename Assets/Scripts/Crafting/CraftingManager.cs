using System.Collections.Generic;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;

public class CraftingManager : MonoBehaviour, IInitableInstance {
    [SerializeField]
    private List<CraftingRecipeConfig> _recipe;

    public void Init() {
        ObsoleteCoreEntryPoint.CraftingManager = this;
    }

    public CraftingRecipeConfig GetRecipe(string uid) {
        return _recipe.FirstOrDefault(r => r.RecipeUid == uid);
    }
}