using UnityEngine;
using UnityEngine.UI;

public class SliderVolumenSincronizador : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // arrastra el AudioSource real aquí

    private void Start()
    {
        float vol = audioSource != null ? audioSource.volume : PlayerPrefs.GetFloat("volumen", 0.5f);
        GetComponent<Slider>().value = vol;
    }
}
