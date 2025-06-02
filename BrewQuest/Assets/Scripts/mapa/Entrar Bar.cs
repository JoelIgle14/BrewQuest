using UnityEngine;
using UnityEngine.SceneManagement;

public class EntarBar : MonoBehaviour
{
    private bool jugadorEnZona = false;

    private void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Scene escenaActual = SceneManager.GetActiveScene();

            if (escenaActual.name == "Nivel1") // <-- Nom exacte de la escena de nivell 1
            {
                SceneManager.LoadScene("PuntuacionFinal");
            }
            else
            {
                int nextSceneIndex = escenaActual.buildIndex + 1;
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextSceneIndex);
                }
                else
                {
                    Debug.Log("Último nivel alcanzado!");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Puerta"))
        {
            jugadorEnZona = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Puerta"))
        {
            jugadorEnZona = false;
        }
    }
}
