using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject fadeBackground;

    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotsParent;

    [Header("Slide Animation")]
    [SerializeField] private float slideSpeed = 8f;
    [SerializeField] private float hiddenPosX = 400f;
    [SerializeField] private float visiblePosX = -10f;

    [Header("Systems")]
    [SerializeField] private ItemExamineUI examineUI;
    [SerializeField] private ItemContextMenu contextMenu; 
    [SerializeField] private PlayerEquipment playerEquipment;

    [SerializeField] private InventoryHealthBar healthBar;



    private InventorySlotUI[] slotsUI;
    private bool isOpen;
    private RectTransform panelRect;
    private Coroutine slideCoroutine;

    private int selectedSlotIndex = -1;
    private bool isMovingItem = false;

    void Start()
    {
        panelRect = inventoryPanel.GetComponent<RectTransform>();
        panelRect.anchoredPosition = new Vector2(hiddenPosX, 0);

        inventoryPanel.SetActive(true);
        fadeBackground.SetActive(false);

        slotsUI = new InventorySlotUI[inventory.maxSlots];
        for (int i = 0; i < inventory.maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsParent);
            slotsUI[i] = slotObj.GetComponent<InventorySlotUI>();
            slotsUI[i].Init(i, this);
        }
        UpdateUI();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            ToggleInventory();
        }
    }

    public void OnSlotClicked(int index)
    {
        var slot = inventory.slots[index];

        Debug.Log("Slot " + index + " isEmpty: " + slot.IsEmpty);
        Debug.Log("contextMenu null? " + (contextMenu == null));
        Debug.Log("Chamando Show agora...");


        if (isMovingItem)
        {
            MoveItem(selectedSlotIndex, index);
            return;
        }

        if (slot.IsEmpty) return;

        selectedSlotIndex = index;
        HighlightSlot(index);

        Vector2 screenPos = slotsUI[index].transform.position;
        Debug.Log("screenPos: " + screenPos);
        contextMenu.Show(slot.item, screenPos);
    }

    public void OnEquipClicked()
    {
        if (selectedSlotIndex < 0) return;

        var slot = inventory.slots[selectedSlotIndex];
        if ( slot.IsEmpty || slot.item.weaponData == null ) return;

        playerEquipment.EquipWeapon(slot.item.weaponData);
        inventory.RemoveItem(slot.item, 1);
        ClearSelection();
        UpdateUI();
    }

    public void OnUseClicked()
    {
        if (selectedSlotIndex < 0) return;

        var slot = inventory.slots[selectedSlotIndex];
        if (slot.IsEmpty || slot.item.itemType != ItemType.Consumable) return;

        var playerHealth = FindAnyObjectByType<Life>();
        if (playerHealth != null )
        {
            playerHealth.AddHP(slot.item.healAmount);
        }

        inventory.RemoveItem(slot.item, 1);
        ClearSelection();
        UpdateUI();

    }

    public void OnExamineClicked()
    {
        if (selectedSlotIndex < 0) return ;

        var slot = inventory.slots[selectedSlotIndex];
        if (!slot.IsEmpty) examineUI.Show(slot.item);

        ClearSelection();
    }

    public void OnMoveClicked()
    {
        if (selectedSlotIndex < 0) return;
        isMovingItem = true;
        contextMenu.Hide();
        HighlightSlot(selectedSlotIndex);
    }

    public void OnCancelClicked()
    {
        ClearSelection();
        contextMenu.Hide();
        examineUI.Hide();
    }

    private void MoveItem(int from, int to)
    {
        if (from == to)
        {
            ClearSelection();
            return;
        }

        var slotFrom = inventory.slots[from];
        var slotTo = inventory.slots[to];

        Item tempItem = slotTo.item;
        int tempQty = slotTo.quantity;

        slotTo.item = slotFrom.item;
        slotTo.quantity = slotFrom.quantity;

        slotFrom.item = tempItem;
        slotFrom.quantity = tempQty;

        if (slotFrom.item == null) slotFrom.Clear();
        if (slotTo.item == null) slotTo.Clear();

        ClearSelection();
        UpdateUI();
    }

    private void HighlightSlot(int index)
    {
        for (int i = 0; i < slotsUI.Length; i++) slotsUI[i].SetHighlight(i == index);
    }

    private void ClearSelection()
    {
        selectedSlotIndex = -1;
        isMovingItem = false;
        for (int i = 0; i < slotsUI.Length; i++) slotsUI[i].SetHighlight(false);
    }


    public void UpdateUI()
    {
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            Debug.Log(slotsUI[i]);
            slotsUI[i].Setup(inventory.slots[i]);
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;

        fadeBackground.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;

        if (isOpen) UpdateUI();
        else OnCancelClicked();

        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlidePanel(isOpen ? visiblePosX : hiddenPosX));
    }

    IEnumerator SlidePanel(float targetX)
    {
        while (Mathf.Abs(panelRect.anchoredPosition.x - targetX) > 0.5f)
        {
            float newX = Mathf.Lerp(panelRect.anchoredPosition.x,targetX, Time.unscaledDeltaTime * slideSpeed);
            panelRect.anchoredPosition = new Vector2(newX, panelRect.anchoredPosition.y);
            yield return null;
        }
        panelRect.anchoredPosition = new Vector2(targetX, panelRect.anchoredPosition.y);
    }
}
