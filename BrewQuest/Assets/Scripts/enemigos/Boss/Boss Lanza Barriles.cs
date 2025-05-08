using System.Collections;
using UnityEngine;

public class BossBarril : MonoBehaviour
{
    public GameObject barrilPrefab;
    public Transform spawnPoint; // Lugar donde salen los barriles
    public float tiempoEntreBarriles = 2f;

    private bool atacando = false;

    public void EmpezarAtaque()
    {
        if (!atacando)
        {
            atacando = true;
            StartCoroutine(LanzarBarriles());
        }
    }

    IEnumerator LanzarBarriles()
    {
        while (true)
        {
            Instantiate(barrilPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(tiempoEntreBarriles);
        }
    }
}
