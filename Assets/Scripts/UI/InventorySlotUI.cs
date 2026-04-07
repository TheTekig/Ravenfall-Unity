using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Image slotHighlight;

    private InventorySlot slot;
    private int slotIndex;
    private InventoryUI inventoryUI;


    public void Init(int index, InventoryUI ui)
    {
        slotIndex = index;
        inventoryUI = ui;
    }

    public void Setup(InventorySlot inventorySlot)
    {
        slot = inventorySlot;

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

    public void SetHighlight(bool active)
    {
        if (slotHighlight != null) slotHighlight.enabled = active;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Slot Clicado : " + slotIndex);
        inventoryUI.OnSlotClicked(slotIndex);
    }
}
