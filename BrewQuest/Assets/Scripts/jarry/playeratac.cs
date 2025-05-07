using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeratac : MonoBehaviour
{
    public float attackRange;
    public float attackDamage;
    public float attacCooldown;

    private float timeToNextAttack = 1.0f;
    public Vector3 positionAttack;


    private bool lookingup;  // Esto es suficiente como un bool, ya que estamos determinando si est� mirando hacia arriba
    private Enemyvida ev;
    Animator animator;

    void Start()
    {
        ev = GetComponent<Enemyvida>();
        animator = GetComponent<Animator>();
        Debug.Log(animator);

    }

    void Update()
    {
        LookingUp();

        if (lookingup)
        {
            positionAttack = transform.position + new Vector3(0f, 0.7f, 0f);
        }
        else
        {
            positionAttack = transform.position + new Vector3(0.6f * Mathf.Sign(transform.localScale.x), 0f, 0f);
        }

        if (Time.time >= timeToNextAttack)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                timeToNextAttack = Time.time + attacCooldown;
                Attack();
            animator.SetTrigger("ataque"); 
            }
        }
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(positionAttack, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("enemy"))
            {
                Enemyvida enemyvid = enemy.GetComponent<Enemyvida>();
                if (enemyvid != null)
                {
                    enemyvid.TakeDamage(attackDamage, gameObject,true);
                    
                    Debug.Log("golpe a enemigo");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(positionAttack, attackRange);
    }

    void LookingUp()
    {
        lookingup = Input.GetKey(KeyCode.UpArrow);
    }
}
