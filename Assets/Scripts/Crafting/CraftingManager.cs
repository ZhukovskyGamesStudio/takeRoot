using System.Collections.Generic;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingManager : MonoBehaviour, IInitableInstance {
    [SerializeField]
    private List<CraftingRecipeConfig> _recipe;

    public void Init() {
        Core.CraftingManager = this;
    }

    public CraftingRecipeConfig GetRecipe(string uid) {
        return _recipe.FirstOrDefault(r => r.RecipeUid == uid);
    }
}