using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playeratac : MonoBehaviour
{
    public float attackRange;
    public float attackDamage;
    public float attacCooldown;
    //hacer un layer de enemigos pa saber si les pegamos
    private float timeToNextAttack = 0f;
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
        positionAttack = transform.position + new Vector3(0.6f, 0f, 0f);  
        
        if (Time.time >= timeToNextAttack)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Attack();
                //nextAttackTime = Time.time + attackCooldown;
                
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
