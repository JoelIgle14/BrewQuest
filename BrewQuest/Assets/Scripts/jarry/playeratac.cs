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
    // Start is called before the first frame update
    
    //public LayerMask enemyLayer;  // Agregar el LayerMask

    void Start()
    {
        //aqui va el link del animador
        
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizamos la posición del ataque
        positionAttack = transform.position + new Vector3(0.6f, 0f, 0f);

        // Comprobamos si ya pasó el tiempo de espera para el siguiente ataque
        if (Time.time >= timeToNextAttack)
        {
            // Si presionamos la tecla de ataque (en este caso X)
            if (Input.GetKeyDown(KeyCode.X))
            {
                Attack();
                // Después de atacar, establecer el tiempo para el próximo ataque
                timeToNextAttack = Time.time + attacCooldown;  // El próximo ataque puede ser después de "attackCooldown" segundos
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
}

    

