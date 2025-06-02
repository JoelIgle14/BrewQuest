using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip[] audios;

    private AudioSource loopSource;    // Para sonidos que hacen loop (ej: caminar)
    private AudioSource sfxSource;     // Para sonidos puntuales (ej: morir, saltar)

    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            loopSource = sources[0];
            sfxSource = sources[1];
        }
        else
        {
            // Crea los audiosources si no existen
            loopSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        Debug.Log("AudioSources asignados: loopSource = " + loopSource + ", sfxSource = " + sfxSource);
    }

    // Sonido en bucle (ej: pasos)
    public void ReproducirLoop(int indice, float volumen)
    {
        if (indice >= 0 && indice < audios.Length)
        {
            if (audios[indice] == null)
            {
                Debug.LogWarning("Audio clip en índice " + indice + " está vacío.");
                return;
            }

            Debug.Log("Intentando reproducir loop clip: " + audios[indice].name);

            if (loopSource.clip != audios[indice])
            {
                loopSource.clip = audios[indice];
                loopSource.loop = true;
                loopSource.volume = volumen;
                loopSource.Play();
                Debug.Log("Clip en loop iniciado.");
            }
            else if (!loopSource.isPlaying)
            {
                loopSource.Play();
                Debug.Log("Clip reiniciado.");
            }
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango: " + indice);
        }
    }
    

    public void PararSonidoLoop()
    {
        loopSource.Stop();
        loopSource.clip = null;
        loopSource.loop = false;
    }

    //  Sonido puntual (ej: muerte, salto)
    public void SeleccionAudio(int indice, float volumen)
    {
        if (indice >= 0 && indice < audios.Length)
        {
            sfxSource.PlayOneShot(audios[indice], volumen);
        }
    }




    public bool EstaReproduciendoLoop(int indice)
    {
        return loopSource.isPlaying && loopSource.clip == audios[indice];
    }
}
