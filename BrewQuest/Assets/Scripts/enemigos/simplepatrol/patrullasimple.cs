using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrullasimple : MonoBehaviour
{
    public float speedwalk = 1f;
    public float contadorT;
    public float tiempocambio;
    public bool esDerecha;
    public bool isBeingHit = false;

    private Enemyvida ev;
    private recibirknockBack recback;
    private Vector3 initialScale;
    private winston winstonScript;

    void Start()
    {
        ev = GetComponent<Enemyvida>();
        recback = GetComponent<recibirknockBack>();
        winstonScript = GetComponent<winston>();
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (ev.golpeado && !isBeingHit)
        {
            isBeingHit = true;
            recback.EmpujeEnemigo();
        }

        if (!ev.golpeado && !isBeingHit)
        {
            Vector3 newScale = transform.localScale;

            if (esDerecha)
            {
                transform.position += Vector3.right * speedwalk * Time.deltaTime;

                if (winstonScript == null || !winstonScript.HaVistoJugador())
                    newScale.x = -Mathf.Abs(initialScale.x);
            }
            else
            {
                transform.position += Vector3.left * speedwalk * Time.deltaTime;

                if (winstonScript == null || !winstonScript.HaVistoJugador())
                    newScale.x = Mathf.Abs(initialScale.x);
            }

            transform.localScale = newScale;

            contadorT -= Time.deltaTime;

            if (contadorT <= 0 && (winstonScript == null || !winstonScript.HaVistoJugador()))
            {
                contadorT = tiempocambio;
                esDerecha = !esDerecha;
            }
        }
    }
}
