using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ataqueDados : MonoBehaviour, Iboss2atac
{
    public GameObject dadoPrefab;
    public Transform[] puntosDeLanzamiento; // Usualmente 2 (izquierda y derecha del jefe)

    public float delayAntesDeLanzar = 0.5f;

    public IEnumerator Execute()
    {
        yield return new WaitForSeconds(delayAntesDeLanzar);

        foreach (Transform punto in puntosDeLanzamiento)
        {
            GameObject dado = Instantiate(dadoPrefab, punto.position, Quaternion.identity);
            // Se le puede asignar una dirección aleatoria diagonal aquí si se quiere
        }

        yield return new WaitForSeconds(1f); // pausa tras ataque
    }
}
