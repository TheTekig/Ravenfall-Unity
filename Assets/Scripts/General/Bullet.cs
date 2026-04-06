using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private int damage = 10; // Dano causado pela bala
    [SerializeField] private float lifeTime = 5f; // Vida útil da bala antes de ser destruída
    [SerializeField] private float groundDelay= 0.5f; // Tempo para destruir a bala após colidir com o chão

    [Header("Collision Tags")]
    [SerializeField] private string enemyTag = "Enemy"; // Tag para identificar os inimigos
    [SerializeField] private string groundTag = "Ground"; // Tag para identificar o chão

    private Rigidbody2D rb;
    private bool hasHitGround = false; // Flag para verificar se a bala já colidiu com o chão
    private int finalDamage; // Dano final da bala, pode ser modificado por buffs ou debuffs

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (finalDamage == 0) finalDamage = damage; // Define o dano final como o dano base se não for modificado

        Destroy(gameObject, lifeTime); // Destrói a bala após o tempo de vida útil
    }

    public void Setup(int weaponDamage)
    {
       finalDamage = weaponDamage; // Configura o dano final da bala com base no dano da arma
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a bala colidiu com um inimigo
        if (collision.gameObject.CompareTag(enemyTag))
        {
            Life life = collision.gameObject.GetComponent<Life>();
            if (life != null)
            {
                life.SubtractHP(finalDamage); // Subtrai o dano da vida do inimigo
            }
            return;
        }

        // Verifica se a bala colidiu com o chão
        if (collision.gameObject.CompareTag(groundTag) && !hasHitGround)
        {
            hasHitGround = true; // Marca que a bala colidiu com o chão
            rb.linearVelocity = Vector2.zero; // Para a bala
            rb.gravityScale = 0; // Remove a gravidade para evitar que a bala caia
            StartCoroutine(DestroyAfterDelay()); // Destrói a bala após o tempo de atraso
        }

    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(groundDelay); // Espera o tempo de atraso
        Destroy(gameObject); // Destrói a bala
    }  

}
