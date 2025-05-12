using System.Collections;
using UnityEngine;

public class BossBarril : MonoBehaviour
{
    public GameObject barrilPrefab;
    public Transform spawnPoint;
    public float tiempoEntreBarriles = 2f;
    public float velocidadHuida = 10f; // 🔥 Aumenta la velocidad de huida
    public float tiempoHastaDestruir = 3f; // ⏱ Tiempo antes de destruir al huir

    private Coroutine barrilCoroutine;
    private bool huyendo = false;

    public void EmpezarAtaque()
    {
        if (barrilCoroutine == null)
        {
            barrilCoroutine = StartCoroutine(LanzarBarriles());
        }
    }

    public void EmpezarHuida()
    {
        // Detiene los barriles
        if (barrilCoroutine != null)
        {
            StopCoroutine(barrilCoroutine);
            barrilCoroutine = null;
        }

        // Comienza la huida
        huyendo = true;

        // 🧨 Destruye al boss después de un tiempo
        Destroy(gameObject, tiempoHastaDestruir);
    }

    void Update()
    {
        if (huyendo)
        {
            transform.Translate(Vector2.right * velocidadHuida * Time.deltaTime);
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
