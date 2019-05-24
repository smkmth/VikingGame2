using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// here is the class which handles all inventory stuff. it should be player agnoistic
/// so npcs, chests, shops or whatever could use a similar / modifed version of this class
/// </summary>
public class Inventory : MonoBehaviour
{
    private Stamina stamina;
    private PlayerInteraction interaction;

    private EquipmentHolder equipmentHolder;

    public List<ItemSlot> itemSlots;

    //set up in editor!
    public GameObject inventoryUI;
    public GameObject itemGrid;

    public int MaxItemSlots;
    public int totalItemsStored;

    //array of the images
    private Image[] itemImages;
    private TextMeshProUGUI[] itemText;

    //how we should display empty inventory slots 
    public Sprite emptySprite;
    public string emptyString;


    public void Start()
    {
        totalItemsStored = 0;
        itemSlots = new List<ItemSlot>();

        for (int i = 0; i < MaxItemSlots; i++)
        {
            itemSlots.Add(new ItemSlot(null, 0, false));
        }


        itemImages = itemGrid.GetComponentsInChildren<Image>();
        foreach (Image image in itemImages)
        {
            image.sprite = emptySprite;
        }
        itemText = itemGrid.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in itemText)
        {
            text.text = emptyString;
        }

        
        equipmentHolder = GetComponent<EquipmentHolder>();
        stamina = GetComponent<Stamina>();     
        interaction = GetComponent<PlayerInteraction>();     
        DisplayInventory();
        inventoryUI.SetActive(false);

    }

    //turn on and off inventory menu
    public void ToggleInventoryMenu()
    {
        
        if (inventoryUI.activeSelf == false)
        {
            inventoryUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            interaction.currentInteractionState = PlayerInteraction.interactionState.InventoryMode;

        }
        else
        {
            inventoryUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            interaction.currentInteractionState = PlayerInteraction.interactionState.Normal;


        }

    }
    
    //gets called by the ui button on click method. FIXME later make this less dependent on editor
    public void SelectInventorySlot(string buttonIndex)
    {
        int selectedIndex = int.Parse(buttonIndex);
        if (itemSlots[selectedIndex].filled)
        {
            Debug.Log("you selected " + selectedIndex + " which corisponds to " + itemSlots[selectedIndex].item.name +
                " You have " + itemSlots[selectedIndex].quantity + " of this item ");

            Item selectedItem = itemSlots[selectedIndex].item;

            switch (selectedItem.type)
            {
                case ItemType.Food:
                    Food foodItem = (Food)selectedItem;
                    stamina.RestoreStamina(foodItem.staminaRestore);
                    RemoveItem(selectedItem);
                    break;

                case ItemType.Weapon:
                    Weapon weaponItem = (Weapon)selectedItem;
                    if (itemSlots[selectedIndex].equiped)
                    {
                        equipmentHolder.UnEquipWeapon();
                        itemSlots[selectedIndex].equiped = false;

                    }
                    else
                    {
                        equipmentHolder.EquipWeapon(weaponItem);
                        itemSlots[selectedIndex].equiped = true;

                    }
                    break;

                case ItemType.Equipment:
                    Equipment equipmentItem = (Equipment)selectedItem;
                    if (!itemSlots[selectedIndex].equiped)
                    {
                        equipmentHolder.EquipItem(equipmentItem);
                        itemSlots[selectedIndex].equiped = true;
                    }
                    else
                    {
                        equipmentHolder.UnEquipItem(equipmentItem);
                        itemSlots[selectedIndex].equiped = false;
                    }
                    break;

            

            }
            
        }
        DisplayInventory();
    }
    
    //JUST update the visuals of the inventory- dont implement any inventory management stuff here please future danny
    public void DisplayInventory()
    {
       
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].filled)
            {
                itemImages[i].sprite = itemSlots[i].item.icon;


                if (itemSlots[i].equiped)
                {
                    var colors = itemImages[i].gameObject.GetComponent<Button>().colors;
                    colors.normalColor = Color.red;
                    colors.highlightedColor = Color.red;
                    itemImages[i].gameObject.GetComponent<Button>().colors = colors;
                    itemText[i].text = itemSlots[i].item.title + "(" + itemSlots[i].quantity + ")" + " Equipped ";
                    itemImages[i].gameObject.GetComponent<Button>().enabled = false;
                    itemImages[i].gameObject.GetComponent<Button>().enabled = true;

                }
                else
                {
                    var colors = itemImages[i].gameObject.GetComponent<Button>().colors;
                    colors.normalColor = Color.white;
                    colors.highlightedColor = Color.white;
                    itemImages[i].gameObject.GetComponent<Button>().colors = colors;
           
                    itemImages[i].gameObject.GetComponent<Button>().enabled = false;
                    itemImages[i].gameObject.GetComponent<Button>().enabled = true;
                    itemText[i].text = itemSlots[i].item.title + "(" + itemSlots[i].quantity + ")";
                }
            
                

            }
            else
            {
                itemText[i].text = emptyString;
                itemImages[i].sprite = emptySprite;

            }
        }
        
    }

    public void AddItem(Item itemToAdd)
    {
        //check we dont already have this item, if not, then add it, else increase the amount we have of it

        for (int i = 0; i < MaxItemSlots; ++i)
        {

            if (itemSlots[i].filled)
            {
                if (itemSlots[i].item.name == itemToAdd.name)
                {
                    itemSlots[i].quantity += 1;
                    totalItemsStored += 1;
                    DisplayInventory();
                    return;
                }

            }
        }
        for (int i = 0; i < MaxItemSlots; ++i)
        {


            if (!itemSlots[i].filled)
            {
                totalItemsStored += 1;
                itemSlots[i].item = itemToAdd;
                itemSlots[i].quantity += 1;
                itemSlots[i].filled = true;
                DisplayInventory();
                return;

            }

        }
     
        Debug.Log("Inventory full!");


    }

    public void RemoveItem(Item itemToRemove)
    {
        if (itemSlots.Count > 0)
        {
            for (int i = 0; i < itemSlots.Count; ++i)
            {
                if (itemSlots[i].item.name == itemToRemove.name)
                {
                    itemSlots[i].quantity -= 1;
                    totalItemsStored -= 1;

                    if (itemSlots[i].quantity <= 0)
                    {
                        itemSlots[i].filled = false;
                        itemSlots[i].item = null;
                     
                    }
                    DisplayInventory();
                    return;
                }
            }
        }
        


    }
}

//this class handles the item slot - wish it could be a struct, but quantity and filled have to be mutable, so :(
public class ItemSlot
{
    public Item item;
    public int quantity;
    public bool filled;
    public bool equiped =false;

    public ItemSlot(Item item, int quantity, bool filled)
    {
        this.item = item;
        this.quantity = quantity;
        this.filled = filled;
    }
}