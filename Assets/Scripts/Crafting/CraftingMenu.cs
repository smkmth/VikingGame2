using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{

    public List<Button> currentSlots;
    public GameObject craftingMenu;
    public GameObject craftingSlotPrefab;
    public GameObject recipesPanel;
    public GameObject missingIngredientsPanel;
    public GameObject foundIngredientsPanel;
    public GameObject ingredientsPrefab;
    public Button craftButton;
    private ButtonHighlighter buttonHighlighter;
    public Crafter thisCrafter;
    private CraftingRecipe selectedItem;

    // Use this for initialization
    void Awake()
    {
        buttonHighlighter = craftingMenu.GetComponent<ButtonHighlighter>();
        thisCrafter = null;
    }

    public void ToggleCraftingMenu(bool isCrafting, Crafter crafter)
    {
        if (isCrafting)
        {
            
            thisCrafter = crafter;
            List<CraftingRecipe> craftingRecipes = thisCrafter.GetCraftingRecipes();
            BuildMenu(craftingRecipes);
            buttonHighlighter.ActivateButtons(currentSlots[0].gameObject);
            UpdateCraftingMenu(selectedItem);

        }
        else
        {
            thisCrafter = null;
        }
        craftingMenu.SetActive(isCrafting);
    }

    public void BuildMenu(List<CraftingRecipe> craftingRecipeList)
    {

        int childCount = recipesPanel.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(recipesPanel.transform.GetChild(i).gameObject);
        }

        foreach (CraftingRecipe recipe in craftingRecipeList)
        {
            CreateCraftingOption(recipe);

        }

    }

    private void CreateCraftingOption(CraftingRecipe craftingRecpie)
    {

        GameObject currentCraftSlot = Instantiate(craftingSlotPrefab, recipesPanel.transform);
        Button currentButton = currentCraftSlot.GetComponent<Button>();
        TextMeshProUGUI currentButtonText = currentCraftSlot.GetComponentInChildren<TextMeshProUGUI>();
        // Tell the button what to do when we press it
        currentButtonText.text = craftingRecpie.itemProduced.title;

        currentButton.onClick.AddListener(delegate
        {
            OnClickCraftSlot(craftingRecpie);
        });
        currentSlots.Add(currentButton);
    }





    public void UpdateCraftingMenu(CraftingRecipe selectedRecipe)
    {
        RemoveChildren();
        List<Item> missingItems = thisCrafter.CheckItemsMissing(selectedRecipe);
        List<Item> ownedItems = thisCrafter.CheckItemsOwned(selectedRecipe);
        foreach (Item missingItem in missingItems)
        {
            GameObject missingItemSlot = Instantiate(ingredientsPrefab, missingIngredientsPanel.transform);
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


    public void OnClickCraftSlot(CraftingRecipe selectedCraftingRecipe)
    {
        selectedItem = selectedCraftingRecipe;
        UpdateCraftingMenu(selectedCraftingRecipe);

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
        currentSlots.Clear();
    }

    void CraftButtonPressed()
    {
        thisCrafter.CraftItem(selectedItem);
        UpdateCraftingMenu(selectedItem);
    }

}
