﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour {

    public interactableType craftingType;
    public Inventory inventory;
    public Inventory playerInventory;
    public List<CraftingRecipe> masterCraftingRecipes;
    public Item currentItemProcessing;

    public List<CraftingRecipe> GetCraftingRecipes()
    {
        List<CraftingRecipe> recipies = new List<CraftingRecipe>();

        for (int i = 0; i < masterCraftingRecipes.Count; i++)
        {
            CraftingRecipe currentCraftingRecipe = masterCraftingRecipes[i];

            if (masterCraftingRecipes[i].requiredType == craftingType)
            {
                recipies.Add(currentCraftingRecipe); 
            }

        }
        return recipies;
    }

    public List<Item> CheckItemsOwned(CraftingRecipe recipeToCheck)
    {
        List<Item> itemsHave = new List<Item>();
        for (int i = 0; i < recipeToCheck.requiredIngredients.Count; i++)
        {
            if (playerInventory.GetItemCount(recipeToCheck.requiredIngredients[i]) != 0)
            {
                itemsHave.Add(recipeToCheck.requiredIngredients[i]);

            }
        }
        return itemsHave;
    }

    public List<Item> CheckItemsMissing(CraftingRecipe recipeToCheck)
    {
        List<Item> itemsMissing = new List<Item>();
        for (int i = 0; i < recipeToCheck.requiredIngredients.Count; i++)
        {
            if (playerInventory.GetItemCount(recipeToCheck.requiredIngredients[i]) == 0)
            {
                itemsMissing.Add(recipeToCheck.requiredIngredients[i]);

            }
        }
        return itemsMissing;
    }

    public int RecipeToIndex(CraftingRecipe recipeToFind)
    {
        int index = -1;
        for (int i = 0; i < masterCraftingRecipes.Count; i++)
        {
            if (masterCraftingRecipes[i].name == recipeToFind.name)
            {
                index = i;
            }
        }
        return index;
    }

    public void CraftItem(CraftingRecipe itemToCraft)
    {
        if (CheckItemsMissing(itemToCraft).Count == 0)
        {
            if (itemToCraft.processTime > 0)
            {
                currentItemProcessing = itemToCraft.itemProduced;
            }
            else
            {
                inventory.AddItem(itemToCraft.itemProduced);

            }
            foreach (Item item in itemToCraft.requiredIngredients)
            {
                playerInventory.RemoveItem(item);
            }
        }
    }

}
