using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAproximidad : MonoBehaviour
{
    private AudioSource audioSource;
    private runfuente rf;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Acceder al objeto padre
        Transform padre = transform.parent;

        // Buscar al hermano llamado "sonido"
        Transform sonidoTransform = padre.Find("animcion");

        if (sonidoTransform != null)
        {
            rf = sonidoTransform.GetComponent<runfuente>();

            if (rf == null)
                Debug.LogWarning("No se encontró el componente 'runfuente' en el objeto 'sonido'.");
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto hermano llamado 'sonido'.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Solo reproducir el audio si canSound es true
        if (rf != null && rf.canSound)
        {
            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        audioSource.Stop();
    }
}
