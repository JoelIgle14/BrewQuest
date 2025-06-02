using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hordas2 : MonoBehaviour, IBossAttack
{
    [Header("Hordas posibles")]
    public List<GameObject> hordaPrefabs;

    [Header("Configuraci�n de invocaci�n")]
    public List<Transform> spawnPoints; // Lista de puntos de aparici�n
    public float delayBeforeSummon = 0.5f;
    public int cantidadInvocaciones = 3;

    public IEnumerator Execute()
    {
        Debug.Log("El jefe est� invocando una horda...");
        yield return new WaitForSeconds(delayBeforeSummon);

        if (hordaPrefabs.Count == 0)
        {
            Debug.LogWarning("No hay hordas configuradas en HordeSummonAttack.");
            yield break;
        }

        if (spawnPoints.Count < cantidadInvocaciones)
        {
            Debug.LogWarning("No hay suficientes puntos de spawn para invocar la cantidad solicitada.");
            yield break;
        }

        // Seleccionamos solo una horda al azar
        int index = Random.Range(0, hordaPrefabs.Count);
        GameObject hordaElegida = hordaPrefabs[index];

        // Seleccionamos puntos de spawn �nicos al azar
        List<Transform> puntosSeleccionados = new List<Transform>();
        List<int> indicesDisponibles = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++) indicesDisponibles.Add(i);

        for (int i = 0; i < cantidadInvocaciones; i++)
        {
            int randIndex = Random.Range(0, indicesDisponibles.Count);
            int spawnIndex = indicesDisponibles[randIndex];
            indicesDisponibles.RemoveAt(randIndex);

            Transform spawn = spawnPoints[spawnIndex];
            Instantiate(hordaElegida, spawn.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f); // peque�a pausa post invocaci�n
    }

    public Vector3? GetDesiredPosition()
{
    return null; // Este ataque no necesita moverse antes de ejecutarse
}

}
