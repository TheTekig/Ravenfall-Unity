using UnityEngine;

public class StaminaSliderFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 1.2f, 0);


    void Start()
    {
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + worldOffset;     
    }
}
