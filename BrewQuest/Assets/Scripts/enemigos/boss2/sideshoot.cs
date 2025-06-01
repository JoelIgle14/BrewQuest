using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sideshoot : MonoBehaviour, Iboss2atac
{
    
    public GameObject pokercoin;

    public GameObject lobullet;

    public List<Transform> spawnPoints; 

   
    public float delayBeforeFire = 0.5f; 
    public int cantidadEspadas = 7; 

    public IEnumerator Execute()
    {
        Random.InitState(System.Environment.TickCount);
        yield return new WaitForSeconds(delayBeforeFire);

        if (pokercoin == null)
        {
            Debug.LogWarning("No se ha asignado el prefab de espada.");
            yield break;
        }

        if (spawnPoints.Count < cantidadEspadas)
        {
            Debug.LogWarning("No hay suficientes puntos de spawn para lanzar la cantidad solicitada de espadas.");
            yield break;
        }

        // Seleccionar puntos únicos al azar
        List<Transform> puntosSeleccionados = new List<Transform>();
        List<int> indicesDisponibles = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++) indicesDisponibles.Add(i);

        for (int i = 0; i < cantidadEspadas; i++)
        {
            int randIndex = Random.Range(0, indicesDisponibles.Count);
            int spawnIndex = indicesDisponibles[randIndex];
            indicesDisponibles.RemoveAt(randIndex);

            Transform spawn = spawnPoints[spawnIndex];
            Instantiate(lobullet, spawn.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f); 
    }
}
