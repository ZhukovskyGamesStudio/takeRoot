using System;
using System.Collections.Generic;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;

[Obsolete]
public class CraftingManager : MonoBehaviour, IInitableInstance {
    [SerializeField]
    private List<CraftingRecipeConfig> _recipe;

    public void Init() {
        ObsoleteCoreEntryPoint.CraftingManager = this;
    }

    public CraftingRecipeConfig GetRecipe(string uid) {
        return null;
        //return _recipe.FirstOrDefault(r => r.RecipeUid == uid);
    }
}