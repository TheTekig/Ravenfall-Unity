using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int actualHP = 1;
    public int ActualHP => actualHP;

    [SerializeField] private UnityEvent<int, int> OnUpdateMaxHP;
    [SerializeField] private UnityEvent<int, int> OnSubtractHP;
    [SerializeField] private UnityEvent<int, int> OnAddHP;
    [SerializeField] private UnityEvent OnDeath;
    [SerializeField] private InventoryHealthBar inventoryHealthBar;

    void Start()
    {
        UpdateMaxHP(maxHP, actualHP);
    }

    public void UpdateMaxHP(int maxHP, int actualHP)
    {
        this.maxHP = maxHP;
        this.actualHP = actualHP;

        OnUpdateMaxHP.Invoke(maxHP, actualHP);
    }

    public void SubtractHP(int amount)
    {
        actualHP -= amount;
        if (actualHP <= 0)
        {
            actualHP = 0;
            OnDeath.Invoke();
        }
        else
        {
            OnSubtractHP.Invoke(maxHP, actualHP);
        }
    }

    public void AddHP(int amount)
    {
        actualHP += amount;
        if (actualHP > maxHP)
        {
            actualHP = maxHP;
        }
        OnAddHP.Invoke(amount, actualHP);
    }


    void Update()
    {
        if (inventoryHealthBar != null)
        {
            if (actualHP > 60)
            {
                inventoryHealthBar.SetState(HealthState.Fine);
            }
            else if (actualHP > 25)
            {
                inventoryHealthBar.SetState(HealthState.Caution);
            }
            else
            {
                inventoryHealthBar.SetState(HealthState.Danger);
            }

        }
    }
}
