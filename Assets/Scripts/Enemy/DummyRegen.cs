using UnityEngine;

public class DummyRegen : MonoBehaviour
{
    private Life life;
    [SerializeField] private int regenAmount = 1;
    [SerializeField] private float regenTimer = 1f;
    void Start()
    {
        life = GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life.ActualHP < 100)
        {
            regenTimer -= Time.deltaTime;
            if (regenTimer <= 0)
            {
                life.AddHP(regenAmount);
                regenTimer = 1f;
            }
        }
    }
}
