using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ItemExamineUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Button closeButton;

    void Start()
    {
        closeButton.onClick.AddListener(Hide);
        Hide();
    }

    public void Show(Item item)
    {
        panel.SetActive(true);
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        if (item.examinePng != null)
        {
            itemImage.sprite = item.examinePng;
        }
        else
        {
            itemImage.sprite = item.itemIcon;
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
