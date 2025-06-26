using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeratac : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public NewBehaviourScript habilidades;

    public float attackRange;
    public float attackDamage;
    public float attacCooldown;
    public float attackDelay;

    private float timeToNextAttack = 0f;
    public Vector3 positionAttack;

    private bool lookingup;
    private bool isAttacking = false; // bandera para evitar ataques múltiples
    private Animator animator;

    private AudioController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        habilidades = GetComponent<NewBehaviourScript>();
        controller = FindObjectOfType<AudioController>();
    }

    void Update()
    {
        if (tutorialManager != null && tutorialManager.DialogoActivo())
            return;

        if (habilidades != null && !habilidades.canAttack)
            return;

        LookingUp();
        CalculateAttackPosition();

        if (Time.time >= timeToNextAttack && !isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isAttacking = true; //  activamos la bandera
                controller.SeleccionAudio(3, 0.2f);
                timeToNextAttack = Time.time + attacCooldown;
                animator.SetTrigger("ataque");
                StartCoroutine(DelayedAttack());
            }
        }
    }

    private IEnumerator DelayedAttack()
    {
        Debug.Log("→ DelayedAttack iniciado");
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("→ DealDamage ejecutado");
        DealDamage();
        isAttacking = false; //  liberamos la bandera después del ataque
    }

    private void DealDamage()
    {
        Debug.Log(" DealDamage() llamado");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(positionAttack, attackRange);
        HashSet<Transform> damagedEnemies = new HashSet<Transform>();

        foreach (Collider2D enemy in hitEnemies)
        {
            if (!enemy.CompareTag("EnemyHitBox")) continue;

            Transform root = enemy.transform.root;
            if (damagedEnemies.Contains(root)) continue;
            damagedEnemies.Add(root);

            // Enemyvida
            Enemyvida enemyvid = enemy.GetComponentInParent<Enemyvida>();
            if (enemyvid != null)
            {
                enemyvid.TakeDamage(attackDamage, gameObject, true);
                Debug.Log("Enemigo normal golpeado");
                continue;
            }

            // BossVida
            BossVida bossVid = enemy.GetComponentInParent<BossVida>();
            if (bossVid != null)
            {
                bossVid.TakeDamage(attackDamage, gameObject, true);
                Debug.Log("Boss golpeado");
                continue;
            }

            // Boss2vida
            Boss2vida boss2Vida = enemy.GetComponentInParent<Boss2vida>();
            if (boss2Vida != null)
            {
                boss2Vida.TakeDamage(attackDamage, gameObject, true);
                Debug.Log("Boss2 golpeado");
                continue;
            }

            // dadosvida
            dadosvida dadosvid = enemy.GetComponentInParent<dadosvida>();
            if (dadosvid != null)
            {
                dadosvid.TakeDamage(attackDamage, gameObject, true);
                Debug.Log("dados golpeado");
            }
        }
    }

    private void LookingUp()
    {
        lookingup = Input.GetKey(KeyCode.UpArrow);
    }

    private void CalculateAttackPosition()
    {
        if (lookingup)
        {
            positionAttack = transform.position + new Vector3(0f, 0.7f, 0f);
        }
        else
        {
            positionAttack = transform.position + new Vector3(1.4f * Mathf.Sign(transform.localScale.x), 0f, 0f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(positionAttack, attackRange);
    }
}
