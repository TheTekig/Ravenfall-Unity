[System.Serializable]
public class InventorySlot 
{
    public Item item;
    public int quantity;

    public bool IsEmpty => item == null;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }

}
