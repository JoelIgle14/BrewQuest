using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeratac : MonoBehaviour
{
    public float attackRange;
    public float attackDamage;
    public float attacCooldown;
    //hacer un layer de enemigos pa saber si les pegamos
    private float timeToNextAttack = 1.0f;
    public Vector3 positionAttack;

    private bool lookingup;
    private Enemyvida ev;

    void Start()
    {
        ev = GetComponent<Enemyvida>();


    }

    // Update is called once per frame
    void Update()
    {
        // Actualizamos la posici�n del ataque
        //positionAttack = transform.position + new Vector3(0.6f, 0f, 0f);

        LookingUp();


        // Calculamos la posici�n de ataque seg�n direcci�n
        if (lookingup)
        {
            // Si mira arriba, atacamos encima del personaje
            positionAttack = transform.position + new Vector3(0f, 0.7f, 0f);
        }
        else
        {
            // Si no, atacamos a los lados dependiendo de hacia d�nde mira
            positionAttack = transform.position + new Vector3(0.6f * Mathf.Sign(transform.localScale.x), 0f, 0f);
        }


        //positionAttack = transform.position + new Vector3(0.6f * Mathf.Sign(transform.localScale.x), 0f, 0f);

        // Comprobamos si ya pas� el tiempo de espera para el siguiente ataque
        if (Time.time >= timeToNextAttack)
        {
            // Si presionamos la tecla de ataque (en este caso X)
            if (Input.GetKeyDown(KeyCode.X))
            {
                Attack();
                // Despu�s de atacar, establecer el tiempo para el pr�ximo ataque
                timeToNextAttack = Time.time + attacCooldown;  // El pr�ximo ataque puede ser despu�s de "attackCooldown" segundos
            }
        }
    }

    void Attack()
    {
        Debug.Log("rustico");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(positionAttack, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            //codigo para restar vida al enemigo
            if (enemy.CompareTag("enemy"))
            {
                Enemyvida enemyvid = enemy.GetComponent<Enemyvida>();
                if (enemyvid != null)
                {
                    enemyvid.TakeDamage(attackDamage);
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
        // Detecta si la tecla vertical est� siendo presionada hacia arriba
        float verticalInput = Input.GetAxisRaw("Vertical");
        lookingup = verticalInput > 0.5f;
    }
}

    

