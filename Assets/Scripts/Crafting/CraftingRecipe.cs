using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftRecipe", menuName = "CraftRecipe", order = 52)]
public class CraftingRecipe : ScriptableObject {


    public List<Item> requiredIngredients;
    public List<int> indexedIngredientQuantity;
    public Item itemProduced;

}

