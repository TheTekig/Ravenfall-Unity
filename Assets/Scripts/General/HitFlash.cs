using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Color hitColor = Color.white; // Cor para indicar que o inimigo foi atingido
    [SerializeField] private float hitDuration = 0.15f; // Duração do efeito de hit
    [SerializeField] private int flashCount = 2; // Quantidade de vezes que o inimigo piscará ao ser atingido

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine hitCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;         
    }


    public void Flash()
    {
        if (hitCoroutine != null) StopCoroutine(hitCoroutine);
        hitCoroutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int  i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(hitDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(hitDuration);
        }
    }
    
}
