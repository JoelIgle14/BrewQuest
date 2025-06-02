using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceattack : MonoBehaviour, Iboss2atac
{
    public GameObject lodice; // Prefab a lanzar
    public List<Transform> spawnPoints; // Debe tener 4 puntos

    public float delayBeforeFire = 0.5f;
    public int cantidadDados = 2;

    public IEnumerator Execute()
    {
        yield return new WaitForSeconds(delayBeforeFire);
        LanzarDados();
        yield return new WaitForSeconds(1f); // Tiempo después del ataque (opcional)
    }

    private void LanzarDados()
    {
        // Seguridad: asegurarse de que hay suficientes puntos
        int maxDados = Mathf.Min(cantidadDados, spawnPoints.Count);

        // Mezclar índices
        List<int> indices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++) indices.Add(i);
        Shuffle(indices);

        // Instanciar en los primeros N puntos aleatorios
        for (int i = 0; i < maxDados; i++)
        {
            Transform spawn = spawnPoints[indices[i]];
            Instantiate(lodice, spawn.position, Quaternion.identity);
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}
