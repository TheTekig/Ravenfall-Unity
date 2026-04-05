using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShooting : MonoBehaviour
{
    private Animator animator;
    private PlayerMoviment movement;
    private PlayerAttack attack;

    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject aimLight;

    [SerializeField] float bulletSpeed = 10f;

    [SerializeField] float aimConeAngle = 45f; // Ângulo do cone de mira em graus
    [SerializeField] float fireRate = 0.5f; // Tempo entre tiros

    private float fireTimer;
    private bool isAiming;

    public bool IsAiming => isAiming; // Expõe se o jogador está mirando para outros scripts


    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMoviment>();
        attack = GetComponent<PlayerAttack>();  
    }

    void Update()
    {
        
        HandleAim();
        HandleShoot();

        aimLight.SetActive(isAiming);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)firePoint.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        aimLight.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HandleAim()
    {
        // Mira ao segurar o botão direito do mouse
        if (Input.GetMouseButton(1) && movement.IsGrounded)
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        animator.SetBool("isAiming", isAiming);
    }

    void HandleShoot()
    {
        if (!isAiming) return;
        if (!movement.IsGrounded || attack.IsAttacking || movement.IsLanding) return; // Exemplo: só atira no chão
        

        fireTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate; // Reseta o timer de tiro
        }
    }


    void Shoot()
    {
        animator.SetTrigger("Shoot");
        animator.Play("Shooting", 2, 0f);
    }

    Vector2 GetAimDirection()
    {
        Vector2 baseDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left; // Direção base do tiro

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = (mousePos - (Vector2)firePoint.position).normalized;

        float angle = Vector2.Angle(baseDirection, directionToMouse);

        if (angle > aimConeAngle)
        {
            float sign = Mathf.Sign(Vector2.SignedAngle(baseDirection, directionToMouse));
            directionToMouse = Quaternion.Euler(0, 0, sign * aimConeAngle) * baseDirection; // Limita a direção dentro do cone de mira
        }

        return directionToMouse;
    }

    public void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 dir = GetAimDirection();
        rb.linearVelocity = dir * bulletSpeed;
    }

}

