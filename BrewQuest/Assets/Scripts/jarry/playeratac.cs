using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeratac : MonoBehaviour
{
    public float attackRange;
    public float attackDamage;
    public float attacCooldown;

    private float timeToNextAttack = 0f;
    public Vector3 positionAttack;

    private bool lookingup;
    private Enemyvida ev;
    private Animator animator;

    void Start()
    {
        ev = GetComponent<Enemyvida>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        LookingUp();
        CalculateAttackPosition();

        if (Time.time >= timeToNextAttack)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Registrar el tiempo para el siguiente ataque
                timeToNextAttack = Time.time + attacCooldown;

                // Lanzar animación
                animator.SetTrigger("ataque");

                // Aplicar el daño inmediatamente
                DealDamage();
            }
        }
    }

    private void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(positionAttack, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("enemy"))
            {
                Enemyvida enemyvid = enemy.GetComponent<Enemyvida>();
                if (enemyvid != null)
                {
                    enemyvid.TakeDamage(attackDamage, gameObject, true);
                    Debug.Log("Enemigo golpeado inmediatamente");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(positionAttack, attackRange);
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
            positionAttack = transform.position + new Vector3(0.6f * Mathf.Sign(transform.localScale.x), 0f, 0f);
        }
    }
}
