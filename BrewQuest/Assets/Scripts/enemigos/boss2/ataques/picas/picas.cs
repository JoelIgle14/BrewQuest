using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class picarain : MonoBehaviour, Iboss2atac
{
    [Header("Configuración de la lluvia")]
    public GameObject pica;
    public float tiempoEntrePicas;
    public float duracion;
    public float minX;
    public float maxX;
    public float alturaSpawn;
    public int cantidadDePicas;

    public IEnumerator Execute()
    {
        //float tiempo = 0f;

        for (int i = 0; i < cantidadDePicas; i++)
        {
            Vector3 posicion = new Vector3(Random.Range(minX, maxX), alturaSpawn, 0);
            Instantiate(pica, posicion, Quaternion.identity);

            yield return new WaitForSeconds(tiempoEntrePicas);
        }

        // Tiempo extra al final si lo deseas
        yield return new WaitForSeconds(0.5f);
    }
}//
