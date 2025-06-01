using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sideshoot : MonoBehaviour, Iboss2atac
{
    public GameObject lobullet;
    public List<Transform> spawnPoints;

    public float delayBeforeFire = 0.5f;
    public int cantidadEspadas = 3; // Prefabs por tanda

    public IEnumerator Execute()
    {
        // Esperamos antes de empezar a disparar
        yield return new WaitForSeconds(delayBeforeFire);

        // Lanzamos 3 tandas con una pausa visible entre ellas
        for (int i = 0; i < 3; i++)
        {
            LanzarTanda();
            yield return new WaitForSeconds(1.85f); // Tiempo entre tandas
        }

        yield return new WaitForSeconds(1f); // Tiempo tras terminar
    }

    private void LanzarTanda()
    {
        // Asegurarnos de no pedir más balas de las que hay puntos
        int maxBalas = Mathf.Min(cantidadEspadas, spawnPoints.Count);

        // Mezclar los índices
        List<int> indices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++) indices.Add(i);
        Shuffle(indices);

        // Instanciar las balas en los primeros 'maxBalas' puntos
        for (int i = 0; i < maxBalas; i++)
        {
            Transform spawn = spawnPoints[indices[i]];
            Instantiate(lobullet, spawn.position, Quaternion.identity);
        }
    }

    // Fisher–Yates Shuffle
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
