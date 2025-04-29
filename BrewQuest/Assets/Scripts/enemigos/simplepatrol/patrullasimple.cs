using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrullasimple : MonoBehaviour
{
    public float patrolDistance = 5f;
    public float speed = 2f;
    private Vector3 startPosition;
    private int direction = 1;
    private recibirknockBack recback;

    public bool isBeingHit = false; // Para controlar si está siendo golpeado

    //otros scripts o componentes
    private Enemyvida ev;

    void Start()
    {
        startPosition = transform.position;
        ev = GetComponent<Enemyvida>();
        recback = GetComponent<recibirknockBack>();
    }

    void Update()
    {
        if (ev.golpeado && !isBeingHit)
        {
            isBeingHit = true; // Activamos que está siendo golpeado
            recback.EmpujeEnemigo();
        }

        if (!ev.golpeado && !isBeingHit)
        {
            {
                transform.position += Vector3.right * direction * speed * Time.deltaTime;
            }


            //flipear
            if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance)
            {
                direction *= -1; // Cambia la dirección

                //esta línea flipea al colega
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
