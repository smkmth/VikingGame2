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
    public GameObject missingIngredientsPanel;
    public GameObject foundIngredientsPanel;
    public GameObject ingredientsPrefab;
    public Inventory playerInventory;

    private CraftingRecipe selectedItem;

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
	public void ToggleCraftingMenu(bool isCrafting)
    {

        craftingMenu.SetActive(isCrafting);
        RemoveChildren();
    }
    public void UpdateCraftingMenu(CraftingRecipe selectedRecipe)
    {
        List<Item> missingItems = CheckItemsMissing(selectedRecipe);
        List<Item> ownedItems = CheckItemsOwned(selectedRecipe);
        foreach (Item missingItem in missingItems)
        {
            GameObject missingItemSlot =Instantiate(ingredientsPrefab, missingIngredientsPanel.transform);
            Image missingItemImage = missingItemSlot.GetComponent<Image>();
            missingItemImage.sprite = missingItem.icon;
        }

        foreach (Item ownedItem in ownedItems)
        {
            GameObject ownedItemSlot = Instantiate(ingredientsPrefab, foundIngredientsPanel.transform);
            Image ownedItemImage = ownedItemSlot.GetComponent<Image>();
            ownedItemImage.sprite = ownedItem.icon;
        }



    }

    public int RecipeToIndex(CraftingRecipe recipeToFind)
    {
        int index = -1;
        for(int i = 0; i < masterCraftingRecipes.Count; i++)
        {
            if (masterCraftingRecipes[i].name == recipeToFind.name)
            {
                index = i;
            }

        }
        return index;
    }
   

    public void OnClickCraftSlot(CraftingRecipe selectedCraftingRecipe)
    {
        RemoveChildren();
        selectedItem = selectedCraftingRecipe;
        UpdateCraftingMenu(selectedCraftingRecipe);

    }
    public void CraftItem()
   {
        if (CheckItemsMissing(selectedItem).Count == 0)
        {
            Debug.Log("HERE");
            playerInventory.AddItem(selectedItem.itemProduced);
        }
    }

    void RemoveChildren()
    {
        selectedItem = null;
        {
            int childCount = missingIngredientsPanel.transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(missingIngredientsPanel.transform.GetChild(i).gameObject);
            }
        }
        {
            int childCount = foundIngredientsPanel.transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                GameObject.Destroy(foundIngredientsPanel.transform.GetChild(i).gameObject);
            }
        }
    }
}
