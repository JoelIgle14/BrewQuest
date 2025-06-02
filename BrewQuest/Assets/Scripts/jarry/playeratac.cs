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

        if (Time.time >= timeToNextAttack)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                controller.SeleccionAudio(3, 0.5f);
                timeToNextAttack = Time.time + attacCooldown;
                animator.SetTrigger("ataque");
                StartCoroutine(DelayedAttack()); //daño tras un delay muy pequeño
            }
        }
    }

    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(attackDelay); // <-- Espera antes de aplicar el daño
        DealDamage();
    }

    private void DealDamage()
    {
        Debug.Log("Evento de animación DealDamage() llamado");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(positionAttack, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("EnemyHitBox"))
            {
                // Primero buscamos Enemyvida (enemigos normales)
                Enemyvida enemyvid = enemy.GetComponentInParent<Enemyvida>();
                if (enemyvid != null)
                {
                    enemyvid.TakeDamage(attackDamage, gameObject, true);
                    Debug.Log("Enemigo normal golpeado");
                    continue;
                }

                // Si no hay Enemyvida, buscamos BossVida
                BossVida bossVid = enemy.GetComponentInParent<BossVida>();
                if (bossVid != null)
                {
                    bossVid.TakeDamage(attackDamage, gameObject, true);
                    Debug.Log("Boss golpeado");
                }

                // Si no hay Enemyvida, buscamos Boss2Vida
                Boss2vida boss2Vida = enemy.GetComponentInParent<Boss2vida>();
                if (boss2Vida != null)
                {
                    boss2Vida.TakeDamage(attackDamage, gameObject, true);
                    Debug.Log("Boss golpeado");
                }

                // Si no hay Enemyvida, buscamos Boss2Vida
                dadosvida dadosvid = enemy.GetComponentInParent<dadosvida>();
                if (dadosvid != null)
                {
                    dadosvid.TakeDamage(attackDamage, gameObject, true);
                    Debug.Log("Boss golpeado");
                }
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
