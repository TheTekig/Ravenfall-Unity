using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Item testAmmo; // Variável para teste temporário

    public int maxSlots = 8;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        for(int i =0; i< maxSlots; i++)
        {
            slots.Add(new InventorySlot());
        }

        if (testAmmo != null) AddItem(testAmmo, 3);
    }

    public bool AddItem(Item item, int quantity)
    {
      foreach( var slot in slots)
      {
        if (!slot.IsEmpty && slot.item == item && item.isStackable)
        {
            if (slot.quantity < item.maxStackSize)
            {
                int spaceLeft = item.maxStackSize - slot.quantity;
                int addQuantity = Mathf.Min(spaceLeft, quantity);

                slot.quantity += addQuantity;
                quantity -= addQuantity;

                if (quantity <= 0) return true; // All items added
            }
        }
      }
      foreach (var slot in slots)
      {
        if (slot.IsEmpty)
        {
            slot.item = item;

            if (item.isStackable)
            {
                int addQuantity = Mathf.Min(item.maxStackSize, quantity);
                slot.quantity = addQuantity;
                quantity -= addQuantity;
            }
            else
            {
                slot.quantity = 1;
                quantity -= 1;
            }

            if (quantity <= 0) return true; // All items added
        }
      }
        return false; // Not all items could be added
    }

    public bool RemoveItem(Item item, int quantity)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item)
            {
                int removeQuantity = Mathf.Min(slot.quantity, quantity);
                slot.quantity -= removeQuantity;
                quantity -= removeQuantity;

                if (slot.quantity <= 0) slot.Clear();

                if (quantity <= 0) return true; // All items removed

            }
        }
        return false; // Not all items could be removed
    }

    public bool HasItem(Item item, int quantity)
    {
        int total = 0;

        foreach(var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item)
            {
                total += slot.quantity;
                if (total >= quantity) return true; // Enough items found
            }
        }
        return false; // Not enough items found
    }

}
