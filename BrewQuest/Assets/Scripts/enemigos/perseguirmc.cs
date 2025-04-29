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

    private void Start()
    {
        target = GameObject.Find("jarry");
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

        if (distanceToPlayer <= chaseDistance)
        {
            // Persecución
            Vector3 direction = target.transform.position - transform.position;

            if (direction.x >= 0.0f)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speedrun * Time.deltaTime);
        }
        else
        {
            // Patrullaje
            if (esDerecha)
            {
                transform.position += Vector3.right * speedwalk * Time.deltaTime;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.position += Vector3.left * speedwalk * Time.deltaTime;
                transform.localScale = new Vector3(1, 1, 1);
            }

            contadorT -= Time.deltaTime;

            if (contadorT <= 0)
            {
                contadorT = tiempocambio;
                esDerecha = !esDerecha;
            }
        }
    }
}
