using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perseguirmc : MonoBehaviour
{
    public float chaseDistance;
    public float speedwalk = 1f;
    public float speedrun = 1.8f;

    public float contadorT;
    public float tiempocambio;
    public bool esDerecha;

    private GameObject target;
    private Vector3 initialScale;

    // Knockback y control de golpe
    private empujechase recback;
    private Enemyvida ev;
    public bool isBeingHit = false;

    private void Start()
    {
        target = GameObject.Find("Jarry");
        initialScale = transform.localScale;

        // Obtener referencias
        ev = GetComponent<Enemyvida>();
        recback = GetComponent<empujechase>();
    }

    private void Update()
    {
        // Verificar si fue golpeado
        if (ev.golpeado && !isBeingHit)
        {
            isBeingHit = true;
            recback.EmpujeEnemigo();
            //isBeingHit = false;
        }

        // Solo ejecutar movimiento si NO está siendo golpeado
        if (!ev.golpeado)
        {
            isBeingHit = false;

            float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

            if (distanceToPlayer <= chaseDistance)
            {
                // Persecución
                Vector3 direction = target.transform.position - transform.position;

                Vector3 newScale = initialScale;
                newScale.x = direction.x >= 0.0f ? -Mathf.Abs(initialScale.x) : Mathf.Abs(initialScale.x);
                transform.localScale = newScale;

                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speedrun * Time.deltaTime);
            }
            else
            {
                // Patrullaje
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
}
