using UnityEngine;

public class ActivarAliado : MonoBehaviour
{
    public GameObject aliado;
    public GameObject Dialogo;
    public MonoBehaviour scriptdialogos; // Aquí se asigna un script como DialogoAliado

    private void Start()
    {
        if (aliado != null)
            aliado.SetActive(false);

        if (Dialogo != null)
            Dialogo.SetActive(false);

        if (scriptdialogos != null)
            scriptdialogos.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (aliado != null)
                aliado.SetActive(true);

            if (Dialogo != null)
                Dialogo.SetActive(true);

            if (scriptdialogos != null)
                scriptdialogos.enabled = true;

            // Si el script de diálogos tiene un método para iniciar diálogo, puedes llamarlo así:
            // ((DialogoAliado)scriptdialogos).IniciarDialogo();

            //Destroy(gameObject); // Evita que se vuelva a activar
        }
    }
}
