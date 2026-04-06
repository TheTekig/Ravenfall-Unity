using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private HitboxController punchHitBox;
    private Animator animator;
    private PlayerMoviment movement; // Novo: impede ataque em certas situações
    private PlayerShooting shooting; // Novo: impede ataque ao atirar

    [SerializeField] float comboResetTime = 1f;
    private int comboStep = 0;
    private float comboTimer;

    private bool canCombo;
    private bool isAttacking;

    [SerializeField] private int attackDamage = 1; // Dano do ataque, pode ser configurado no Inspector
    public bool IsAttacking => isAttacking; // Expõe se está atacando para outros scripts

   

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMoviment>();
        shooting = GetComponent<PlayerShooting>();
        
        
    }

    void Update()
    {
        bool canAttack = movement.IsGrounded && !movement.IsLanding && !shooting.IsAiming; // Exemplo de condições para permitir ataque

        // Cancela o ataque se o jogador começar a se mover
        if (isAttacking && movement.IsMoving)
        {
            CancelAttack();
            return;
        }

        // Inicia ou continua o combo de ataque
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            HandleAttack();
        }

        // Reset do combo por tempo
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            comboStep = 0;
        }

        // Controla hitbox pelo estado da animação na AttackLayer
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(1); // Layer 1 = AttackLayer
        bool inAttackAnim = state.IsName("Attack1") || state.IsName("Attack2") || state.IsName("Attack3");


        if (!inAttackAnim && comboTimer <= 0)
        {
            isAttacking = false;
            canCombo = false;
        }
    }

    void HandleAttack()
    {
        //Primeiro Ataque
        if (!isAttacking)
        {
            StartAttack(1);
            return;
        }

        // Continua o combo
        if (canCombo)
        {
            int nextStep = comboStep + 1;
            if (nextStep > 3) nextStep = 1;// Supondo que temos 3 ataques no combo
            StartAttack(nextStep);
        }
    }

    void StartAttack(int step)
    {
        isAttacking = true;
        canCombo = false;
        comboStep = step;
        comboTimer = comboResetTime;

        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");

        animator.SetTrigger("Attack" + step);
    }

    void CancelAttack()
    {
        isAttacking = false;
        canCombo = false;
        comboStep = 0;
        


        // Reseta os triggers de ataque para não ficarem pendentes na fila do Animator
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");

        // Força a AttackLayer a voltar para o estado vazio (Empty)
        animator.Play("Empty", 1, 0f);
    }

    // Chamado por Animation Event no meio da animação de ataque
    public void EnableCombo()
    {
        canCombo = true;
    }

    // Chamado por Animation Event no final da animação de ataque
    public void EndAttack()
    {
        if (!canCombo) isAttacking = false;
    }

    public void ApplyAttackDamage()
    {
        Debug.Log("Aplicando dano de ataque: " + attackDamage);
        punchHitBox.ApplyDamage(attackDamage);
    }
}