using System.Collections;
using UnityEngine;

public class ZonaSimbolo : MonoBehaviour
{
    public GameObject simboloUI; // arrástralo desde la escena o prefab

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MostrarSimbolo());
        }
    }

    IEnumerator MostrarSimbolo()
    {
        simboloUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        simboloUI.SetActive(false);
    }
}
