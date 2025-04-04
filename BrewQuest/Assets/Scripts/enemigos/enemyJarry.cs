using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float patrolDistance = 5f;
    public float speed = 2f;

    //public int jevida;
    //public bool jevivo;
    
    private Rigidbody2D rb;
    

    private Vector3 startPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance)
        {
            direction *= -1; // Cambia la dirección
            
            //esta línea flipea al colega
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); 
        }
    }

    void RecibirDaño()
    {
  
    }
}


