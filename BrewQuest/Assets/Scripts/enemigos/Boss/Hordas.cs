using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hordas : MonoBehaviour, IBossAttack
{
    [Header("Hordas posibles")]
    public List<GameObject> hordaPrefabs;

    [Header("Configuración de invocación")]
    public Transform spawnPoint;
    public float delayBeforeSummon = 0.5f;

    public IEnumerator Execute()
    {
        Debug.Log("El jefe está invocando una horda...");
        yield return new WaitForSeconds(delayBeforeSummon);

        if (hordaPrefabs.Count == 0)
        {
            Debug.LogWarning("No hay hordas configuradas en HordeSummonAttack.");
            yield break;
        }

        int index = Random.Range(0, hordaPrefabs.Count);
        Instantiate(hordaPrefabs[index], spawnPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(1f); // pequeña pausa post invocación
    }
}
