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
    [SerializeField] private float hiddenPosX = 220f;
    [SerializeField] private float visiblePosX = -10f;


    private InventorySlotUI[] slotsUI;
    private bool isOpen;
    private RectTransform panelRect;
    private Coroutine slideCoroutine;

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
        }
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
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

        fadeBackground.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;

        if (isOpen) UpdateUI();

        if(slideCoroutine != null) StopCoroutine(slideCoroutine);
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
