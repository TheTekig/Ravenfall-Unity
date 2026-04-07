using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button examineButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button moveButton;
    [SerializeField] private Button useButton;

    private InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
        Hide();

        equipButton.onClick.AddListener(() => { inventoryUI.OnEquipClicked(); Hide(); });
        examineButton.onClick.AddListener(() => { inventoryUI.OnExamineClicked(); Hide(); });
        moveButton.onClick.AddListener(() => { inventoryUI.OnMoveClicked(); Hide(); });
        cancelButton.onClick.AddListener(() => { inventoryUI.OnCancelClicked(); Hide(); });
        useButton.onClick.AddListener(() => { inventoryUI.OnUseClicked(); Hide(); });
    }

    public void Show(Item item, Vector2 position)
    {
        Debug.Log("Show() chamado! panel null? " + (panel == null));
        panel.SetActive(true);
        transform.position = position;

        equipButton.gameObject.SetActive(item.itemType == ItemType.Weapon && item.weaponData != null);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

}
