using System.Collections;
using UnityEngine;

public class destroyhab2 : MonoBehaviour
{
    public GameObject iconoDobleSaltoUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivarDobleSaltoYDestruir(0.1f));
        }
    }

    private IEnumerator ActivarDobleSaltoYDestruir(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (iconoDobleSaltoUI != null)
        {
            iconoDobleSaltoUI.SetActive(true);
        }

        GameManager.Instance.hasDoubleJump = true;

        Destroy(gameObject);
        Debug.Log("Doble salto activado y objeto destruido.");
    }
}
