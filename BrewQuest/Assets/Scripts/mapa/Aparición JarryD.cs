using UnityEngine;

public class ActivarAliado : MonoBehaviour
{
    public GameObject aliado;
    public GameObject panelDialogo;

    private void Start()
    {
        aliado.SetActive(false);
        panelDialogo.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            aliado.SetActive(true);
            panelDialogo.SetActive(true);
        }
    }
}
