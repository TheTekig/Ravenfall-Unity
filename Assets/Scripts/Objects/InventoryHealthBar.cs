using UnityEngine;
using UnityEngine.UI;

// Estados de saude visiveis na barra do inventario
public enum HealthState
{
    Fine,
    Caution,
    Danger
}

// Coloca esse script no objeto da barra de vida no inventario.
// Chame SetState() a partir do seu sistema de vida existente.
public class InventoryHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    [Header("Cores por estado")]
    [SerializeField] private Color colorFine = new Color(0.24f, 0.86f, 0.52f); // Verde
    [SerializeField] private Color colorCaution = new Color(0.94f, 0.75f, 0.16f); // Amarelo
    [SerializeField] private Color colorDanger = new Color(0.86f, 0.24f, 0.24f); // Vermelho

    // Chame este metodo do seu PlayerHealth (ou similar) passando o estado atual
    public void SetState(HealthState state)
    {
        switch (state)
        {
            case HealthState.Fine:
                healthBarImage.color = colorFine;
                break;
            case HealthState.Caution:
                healthBarImage.color = colorCaution;
                break;
            case HealthState.Danger:
                healthBarImage.color = colorDanger;
                break;
        }
    }
}