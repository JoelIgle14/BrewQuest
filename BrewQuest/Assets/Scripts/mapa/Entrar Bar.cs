using UnityEngine;
using UnityEngine.SceneManagement;

public class EntarBar : MonoBehaviour
{
    private bool jugadorEnZona = false;

    private void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.UpArrow))
        {
            string escenaActual = SceneManager.GetActiveScene().name;

            if (escenaActual == "PuntuacionFinal")
            {
                // 👉 Si estem a la escena de puntuació, passem als crèdits
                SceneManager.LoadScene("CreditosFinales");
            }
            else
            {
                // 👉 Si no, seguim amb el comportament normal (canviar de nivell)
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
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
