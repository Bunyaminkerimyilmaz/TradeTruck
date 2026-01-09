using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;


    [System.Serializable]
    public class InventorySlot
    {
        public ItemData itemData;
        public int count;
    }

    public List<InventorySlot> myInventory = new List<InventorySlot>();

    private void Awake()
    {
        Instance = this;
    }


    public string GetInventoryString()
    {
        string text = "";
        foreach (var slot in myInventory)
        {
            if (slot.itemData != null)
            {
                text += slot.itemData.itemName + " x " + slot.count + "\n";
            }
        }

        return text;
    }


    public void AddItem(ItemData item, int amount)
    {
        foreach (var slot in myInventory)
        {
            if (slot.itemData == item)
            {
                slot.count += amount;
                return;
            }
        }

        InventorySlot newSlot = new InventorySlot();
        newSlot.itemData = item;
        newSlot.count = amount;
        myInventory.Add(newSlot);
    }

    public int GetItemCount(ItemData itemToCheck)
    {
        foreach (var slot in myInventory)
        {
            if (slot.itemData == itemToCheck)
            {
                return slot.count;
            }
        }

        return 0;
    }

    public bool HasEnoughItem(ItemData item, int amount)
    {
        foreach (var slot in myInventory)
        {
            if (slot.itemData == item)
            {
                return slot.count >= amount;
            }
        }

        return false;
    }


    public void RemoveItem(ItemData item, int amount)
    {
        foreach (var slot in myInventory)
        {
            if (slot.itemData == item)
            {
                slot.count -= amount;
                if (slot.count < 0) slot.count = 0;
                return;
            }
        }
    }
}