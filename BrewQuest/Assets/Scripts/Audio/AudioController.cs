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
    }

    // Sonido en bucle (ej: pasos)
    public void ReproducirLoop(int indice, float volumen)
    {
        if (indice >= 0 && indice < audios.Length)
        {
            if (loopSource.clip != audios[indice])
            {
                loopSource.clip = audios[indice];
                loopSource.loop = true;
                loopSource.volume = volumen;
                loopSource.Play();
            }
            else if (!loopSource.isPlaying)
            {
                loopSource.Play();
            }
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
