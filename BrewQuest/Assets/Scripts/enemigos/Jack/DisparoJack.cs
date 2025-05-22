using System.Collections;  //  Necesario para IEnumerator
using System.Collections.Generic;
using UnityEngine;

public class DisparoJack : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;

    public float tiempoEntreRafagas = 2f;
    public float tiempoEntreBalas = 0.2f;

    public int balasPorRafaga = 3;

    private bool disparando = false;

    void Start()
    {
        InvokeRepeating("IniciarRafaga", 1f, tiempoEntreRafagas);
    }

    void IniciarRafaga()
    {
        if (!disparando)
        {
            StartCoroutine(DispararRafaga());
        }
    }

    IEnumerator DispararRafaga()
    {
        disparando = true;

        for (int i = 0; i < balasPorRafaga; i++)
        {
            DispararBala();
            yield return new WaitForSeconds(tiempoEntreBalas);
        }

        disparando = false;
    }

    void DispararBala()
    {
        Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
    }
}
