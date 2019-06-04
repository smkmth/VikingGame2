using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Button craftButton;
    public Inventory playerInventory;

    private CraftingRecipe selectedItem;

    // Use this for initialization
    void Start () {
        playerInventory = GetComponent<Inventory>();
        for(int i = 0; i  < masterCraftingRecipes.Count; i++)
        {
            GameObject currentCraftSlot = Instantiate(craftingSlotPrefab, recipesPanel.transform);
            Button currentButton = currentCraftSlot.GetComponent<Button>();
            TextMeshProUGUI currentButtonText = currentCraftSlot.GetComponentInChildren<TextMeshProUGUI>();
            CraftingRecipe currentCraftingRecipe = masterCraftingRecipes[i];
            // Tell the button what to do when we press it
            currentButtonText.text = currentCraftingRecipe.itemProduced.title;

            currentButton.onClick.AddListener(delegate
            {
                OnClickCraftSlot(currentCraftingRecipe);
            });
            currentSlots.Add(currentButton);
        }
        craftingMenu.SetActive(false);
        selectedItem = masterCraftingRecipes[0];

		
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
        UpdateCraftingMenu(selectedItem);
    }

    public void UpdateCraftingMenu(CraftingRecipe selectedRecipe)
    {
        RemoveChildren();

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
        if (missingItems.Count == 0)
        {
            craftButton.interactable = true;
        }
        else
        {
            craftButton.interactable = false;

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
        selectedItem = selectedCraftingRecipe;
        UpdateCraftingMenu(selectedCraftingRecipe);

    }
    public void CraftItem()
   {
        if (CheckItemsMissing(selectedItem).Count == 0)
        {
            playerInventory.AddItem(selectedItem.itemProduced);
            foreach (Item item in selectedItem.requiredIngredients)
            {
                playerInventory.RemoveItem(item);
            }
        }
        UpdateCraftingMenu(selectedItem);

    }

    void RemoveChildren()
    {
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
