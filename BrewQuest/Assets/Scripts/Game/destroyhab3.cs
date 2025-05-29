using System.Collections;
using UnityEngine;

public class destroyhab3 : MonoBehaviour
{
    public GameObject iconoMangueraUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivarMangueraYDestruir(0.1f));
        }
    }

    private IEnumerator ActivarMangueraYDestruir(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (iconoMangueraUI != null)
        {
            iconoMangueraUI.SetActive(true);
        }

        GameManager.Instance.hasShoot = true;

        Destroy(gameObject);
    }
}
