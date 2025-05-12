using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyhab : MonoBehaviour
{
    public GameObject barraUI;
    public GameObject mensajeDashPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivarBarraYDestruir(0.1f));
        }
    }

    private IEnumerator ActivarBarraYDestruir(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (barraUI != null)
        {
            barraUI.SetActive(true);
        }

        if (mensajeDashPanel != null)
        {
            mensajeDashPanel.SetActive(true);
            yield return new WaitForSeconds(3f);
            mensajeDashPanel.SetActive(false);
        }

        Destroy(gameObject);
        Debug.Log("Barra activada, mensaje mostrado, y objeto destruido.");
    }
}
