using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeratac : MonoBehaviour
{
    public Vector2 attackBoxSize = new Vector2(1f, 1f);  // Tamaño del área de ataque


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
        CalculateAttackPosition();

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
                animator.SetTrigger("ataque");
            }
        }

    }

    public void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(positionAttack, attackBoxSize, 0f);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("enemy"))
            {
                Enemyvida enemyvid = enemy.GetComponent<Enemyvida>();
                if (enemyvid != null)
                {
                    enemyvid.TakeDamage(attackDamage, gameObject, true);
                    Debug.Log("golpe a enemigo");
                }
            }
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(positionAttack, attackBoxSize);
        CalculateAttackPosition();
    }


    void LookingUp()
    {
        lookingup = Input.GetKey(KeyCode.UpArrow);
    }

    void CalculateAttackPosition()
    {
        if (Application.isPlaying)
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
        else
        {
            // Asume mirando hacia la derecha y no hacia arriba cuando estás en modo editor
            positionAttack = transform.position + new Vector3(0.6f, 0f, 0f);
        }
    }

}

