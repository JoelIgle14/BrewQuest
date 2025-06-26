using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Bar1"); // Reemplaza con el nombre de la escena del juego
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego..."); // Esto aparecerá en el editor
        Application.Quit(); // Esto cerrará la aplicación cuando esté compilada
    }
}
