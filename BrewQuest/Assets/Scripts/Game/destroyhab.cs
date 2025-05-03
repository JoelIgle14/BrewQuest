using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyhab : MonoBehaviour
{
    public GameObject barraUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivarBarraYDestruir(0.5f));
        }
    }

    private IEnumerator ActivarBarraYDestruir(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (barraUI != null)
        {
            barraUI.SetActive(true);
        }

        Destroy(gameObject);
        Debug.Log("Barra activada y objeto destruido después de retraso.");
    }
}
