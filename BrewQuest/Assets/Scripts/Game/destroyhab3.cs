using System.Collections;
using UnityEngine;

public class destroyhab3 : MonoBehaviour
{
    public GameObject iconoMangueraUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivarMangueraYDestruir(0.1f, collision.gameObject));
        }
    }

    private IEnumerator ActivarMangueraYDestruir(float delay, GameObject player)
    {
        yield return new WaitForSeconds(delay);

        if (iconoMangueraUI != null)
        {
            iconoMangueraUI.SetActive(true);
        }

        GameManager.Instance.hasShoot = true;

        // Activar el script de disparo si está presente
        Disparo disparo = player.GetComponent<Disparo>();
        if (disparo != null)
        {
            disparo.ActivarPowerUp();
            Debug.Log("Disparo activado al recoger la manguera.");
        }

        Destroy(gameObject);
    }
}
