using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles UI")]
    public GameObject panelMenu;     // El panel con los botones Play, Quit, Ajustes
    public GameObject panelAjustes;  // El panel con el slider de volumen y volver
    public void PlayGame()
    {
        SceneManager.LoadScene("Bar1"); // Reemplaza con el nombre de tu escena
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void MostrarAjustes()
    {
        panelMenu.SetActive(false);
        panelAjustes.SetActive(true);
        
    }

    public void VolverAlMenu()
    {
        panelAjustes.SetActive(false);
        panelMenu.SetActive(true);
    }
}
