using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrullasimple : MonoBehaviour
{
    public float speedwalk = 1f;
    public float contadorT;
    public float tiempocambio;
    public bool esDerecha;
    public bool isBeingHit = false; // Para controlar si está siendo golpeado

    //otros scripts o componentes
    private Enemyvida ev;
    private recibirknockBack recback;
    private Vector3 initialScale;

    void Start()
    {
        ev = GetComponent<Enemyvida>();
        recback = GetComponent<recibirknockBack>();
        initialScale = transform.localScale;
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
            Vector3 newScale = initialScale;

            if (esDerecha)
            {
                transform.position += Vector3.right * speedwalk * Time.deltaTime;
                newScale.x = -Mathf.Abs(initialScale.x);
            }
            else
            {
                transform.position += Vector3.left * speedwalk * Time.deltaTime;
                newScale.x = Mathf.Abs(initialScale.x);
            }

            transform.localScale = newScale;

            contadorT -= Time.deltaTime;

            if (contadorT <= 0)
            {
                contadorT = tiempocambio;
                esDerecha = !esDerecha;
            }
        }
    }
}

/*
 * 
 // Patrullaje
                */
