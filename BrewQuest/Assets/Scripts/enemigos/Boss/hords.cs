using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hords : MonoBehaviour, IBossAttack
{
    [Header("Hordas posibles")]
    public List<GameObject> hordaPrefabs;

    [Header("Configuraci�n de invocaci�n")]
    public Transform spawnPoint;
    public float delayBeforeSummon = 0.5f;

    public IEnumerator Execute()
    {
        Debug.Log("El jefe est� invocando una horda...");
        yield return new WaitForSeconds(delayBeforeSummon);

        if (hordaPrefabs.Count == 0)
        {
            Debug.LogWarning("No hay hordas configuradas en HordeSummonAttack.");
            yield break;
        }

        int index = Random.Range(0, hordaPrefabs.Count);
        Instantiate(hordaPrefabs[index], spawnPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(1f); // peque�a pausa post invocaci�n
    }

        public Vector3? GetDesiredPosition()
{
    return null; // Este ataque no necesita moverse antes de ejecutarse
}

}
