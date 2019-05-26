using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// here is the class which handles all inventory stuff. it should be player agnoistic
/// so npcs, chests, shops or whatever could use a similar / modifed version of this class
/// </summary>
public class Inventory : MonoBehaviour
{

    public List<ItemSlot> itemSlots;

    //26 item slots
    public int MaxItemSlots;
    public int totalItemsStored;

    // Use this for initialization
    void Start()
    {
        totalItemsStored = 0;

        itemSlots = new List<ItemSlot>();

        for (int i = 0; i <= MaxItemSlots; i++)
        {
            itemSlots.Add(new ItemSlot(null, 0, false));
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
                return;

            }

        }

        Debug.Log("Inventory full!");


    }

    public void RemoveItem(Item itemToRemove)
    {
        if (itemSlots.Count > 0)
        {
            for (int i = 0; i < MaxItemSlots; ++i)
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
                    return;
                }
            }
        }



    }

}
//this class handles the item slot - wish it could be a struct, but quantity and filled have to be mutable, so :(
[System.Serializable]
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