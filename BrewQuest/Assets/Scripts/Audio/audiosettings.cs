using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audiosettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private float volume;

    private void Update()
    {
        float db = Mathf.Log10(Mathf.Clamp(volume, 0.00001f, 1f)) * 20;
        audioMixer.SetFloat("AmbientVolume", db); //nombre
    }
}
