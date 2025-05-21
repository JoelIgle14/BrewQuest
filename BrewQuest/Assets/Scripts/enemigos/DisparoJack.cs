using UnityEngine;

public class DisparoEnemigo : MonoBehaviour
{
    public GameObject balaPrefab;  // Arrastra aquí tu prefab BalaMoneda
    public Transform puntoDisparo; // Un hijo del enemigo donde aparece la bala
    public float tiempoEntreDisparos = 2f;

    private float tiempoDisparo;

    void Update()
    {
        tiempoDisparo += Time.deltaTime;
        if (tiempoDisparo >= tiempoEntreDisparos)
        {
            Disparar();
            tiempoDisparo = 0f;
        }
    }

    void Disparar()
    {
        Instantiate(balaPrefab, puntoDisparo.position, Quaternion.identity);
    }
}
