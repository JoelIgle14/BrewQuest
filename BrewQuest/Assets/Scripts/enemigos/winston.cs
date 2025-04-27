using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winston : MonoBehaviour
{
    public float patrolDistance = 5f;
    public float speed = 2f;

    //logica de golpe
    public float golpeForce = 3f; // fuerza del empuj�n
    public float golpeDuration = 2f; // cu�nto dura el empuj�n

    private bool isBeingHit = false; // Para controlar si est� siendo golpeado

    private Vector3 startPosition;
    private int direction = 1;

    //otros scripts
    private Enemyvida ev;

    void Start()
    {
        startPosition = transform.position;
        ev = GetComponent<Enemyvida>();
    }

    void Update()
    {
        if (ev.golpeado && !isBeingHit)
        {
            isBeingHit = true; // Activamos que est� siendo golpeado
            StartCoroutine(GolpeCoroutine());
        }

        if (!ev.golpeado && !isBeingHit)
        {
            {
                transform.position += Vector3.right * direction * speed * Time.deltaTime;
            }


            //flipear
            if (Mathf.Abs(transform.position.x - startPosition.x) >= patrolDistance)
            {
                direction *= -1; // Cambia la direcci�n

                //esta l�nea flipea al colega
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }


        IEnumerator GolpeCoroutine()
        {
            float timer = 0f;

            // Mientras dure el golpe, empujamos al enemigo
            while (timer < golpeDuration)
            {
                transform.position += Vector3.left * golpeForce * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null; // Espera un frame
            }

            // Despu�s de que el golpe termine, reiniciamos las variables
            isBeingHit = false; // El enemigo ya no est� siendo golpeado
            ev.golpeado = false; // Reiniciamos el estado de golpe
        }
    }
}
