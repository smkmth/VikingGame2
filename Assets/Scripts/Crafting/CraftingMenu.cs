using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour {

    public List<CraftingRecipe> masterCraftingRecipes;
    public List<Button> currentSlots;
    public GameObject craftingMenu;
    public GameObject craftingSlotPrefab;
    public GameObject recipesPanel;
    public GameObject ingredientsPanel;
    public GameObject ingredientsPrefab;
    public Inventory playerInventory;

    // Use this for initialization
    void Start () {
        playerInventory = GetComponent<Inventory>();
        for(int i = 0; i  < masterCraftingRecipes.Count; i++)
        {
            GameObject currentCraftSlot = Instantiate(craftingSlotPrefab, recipesPanel.transform);
            Button currentButton = currentCraftSlot.GetComponent<Button>();
            CraftingRecipe currentCraftingRecipe = masterCraftingRecipes[i];
            // Tell the button what to do when we press it
            
            currentButton.onClick.AddListener(delegate
            {
                OnClickCraftSlot(currentCraftingRecipe);
            });
            currentSlots.Add(currentButton);
        }
        craftingMenu.SetActive(false);

		
	}

    public List<Item> CheckToBuild(CraftingRecipe recipeToCheck)
    {
        
        List<Item> itemMissing = new List<Item>();
        for (int i = 0; i < recipeToCheck.requiredIngredients.Count; i++)
        {
            if (playerInventory.GetItemCount(recipeToCheck.requiredIngredients[i]) == 0)
            {
                itemMissing.Add(recipeToCheck.requiredIngredients[i]);

            }
        }
        return itemMissing;
    }
	public void ToggleCraftingMenu(bool isCrafting)
    {

        craftingMenu.SetActive(isCrafting);
        UpdateCraftingMenu();



    }
    public void UpdateCraftingMenu()
    {
        List<Item> missingItems = CheckToBuild(masterCraftingRecipes[0]);
        foreach (Item missingItem in missingItems)
        {
            GameObject missingItemSlot =Instantiate(ingredientsPrefab, ingredientsPanel.transform);
            Image missingItemImage = missingItemSlot.GetComponent<Image>();
            missingItemImage.sprite = missingItem.icon;
        }


        
    }
   

    public void OnClickCraftSlot(CraftingRecipe selectedCraftingRecipe)
    {

    }
}
