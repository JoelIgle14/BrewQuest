using UnityEngine;
using System.Collections;

public class DisparoEnemigo2 : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform puntoDisparo;

    public int balasPorRafaga = 3;
    public float tiempoEntreBalas = 0.2f;
    public float tiempoEntreRafagas = 2f;

    private bool puedeDisparar = true;

    void Update()
    {
        if (puedeDisparar)
        {
            StartCoroutine(DispararRafaga());
        }
    }

    IEnumerator DispararRafaga()
    {
        puedeDisparar = false;

        for (int i = 0; i < balasPorRafaga; i++)
        {
            Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
            yield return new WaitForSeconds(tiempoEntreBalas);
        }

        yield return new WaitForSeconds(tiempoEntreRafagas);
        puedeDisparar = true;
        Debug.Log("Disparo bala");
        Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);

    }
}
