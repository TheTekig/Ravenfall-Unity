using UnityEditor;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject fadeBackground;

    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotsParent;

    private InventorySlotUI[] slotsUI;

    private bool isOpen;

    void Start()
    {
        inventoryPanel.SetActive(false);
        fadeBackground.SetActive(false);

        

        slotsUI = new InventorySlotUI[inventory.maxSlots];
        for (int i = 0; i < inventory.maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsParent);
            slotsUI[i] = slotObj.GetComponent<InventorySlotUI>();
        }
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
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

        inventoryPanel.SetActive(isOpen);
        fadeBackground.SetActive(isOpen);
        // Pausa o jogo quando o inventário está aberto
        Time.timeScale = isOpen ? 0f : 1f;
    }
}
