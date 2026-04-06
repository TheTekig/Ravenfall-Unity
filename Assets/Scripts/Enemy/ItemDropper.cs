using UnityEngine;


    public class ItemDropper : MonoBehaviour
{

    [System.Serializable]
    public class DropEntry
    {
        public Item item;
        public int quantity;
        [Range(0f, 1f)] public float dropChance = 0.5f;
    }

        [SerializeField] private GameObject itemPickupPrefab;
        [SerializeField] private DropEntry[] drops;
    

        public void DropItems()
        {
            foreach (var drop in drops)
            {
                if (Random.value <= drop.dropChance)
                {
                    GameObject pickup = Instantiate(itemPickupPrefab,transform.position, Quaternion.identity);
                    ItemPickup itemPickup = pickup.GetComponent<ItemPickup>();
                    itemPickup.item = drop.item;
                    itemPickup.quantity = drop.quantity;
                }

            }
        }
    }

    

