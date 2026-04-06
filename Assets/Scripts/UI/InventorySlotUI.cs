using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;


    public void Setup(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            itemIcon.enabled = false;
            quantityText.text = "";
            return;
        }
        itemIcon.enabled = true;
        itemIcon.sprite = slot.item.itemIcon;

        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
    
    }
}
