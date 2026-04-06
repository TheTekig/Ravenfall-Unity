using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class ItemPickup : MonoBehaviour
{
    [Header("Item")]
    public Item item;
    public int quantity = 1;

    [Header("Referencias")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private Light2D itemLight;

    [Header("Configuracoes da Luz")]
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minIntensity = 0.8f;
    [SerializeField] private float maxIntensity = 1.5f;

    private bool playerInRange = false;
    private Inventory playerInventory;

    void Start()
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (itemLight != null)
        {
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            itemLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (playerInventory == null) return;

        bool added = playerInventory.AddItem(item, quantity);
        if (added) 
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventario Cheio!");
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = collision.GetComponent<Inventory>();
            if (playerInventory == null)  playerInventory = collision.GetComponentInParent<Inventory>();
            if (playerInventory == null) playerInventory = collision.GetComponentInChildren<Inventory>();

            if (interactPrompt != null) interactPrompt.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            if (interactPrompt != null) interactPrompt.SetActive(false);
        }

    }
}
