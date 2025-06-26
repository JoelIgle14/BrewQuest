using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;  
    [Header("Paneles UI")]
    public GameObject panelMenu;     // El panel con los botones Play, Quit, Ajustes
    public GameObject panelAjustes;  // El panel con el slider de volumen y volver
    
    void Start()
    {
        // Al iniciar, solo el panel principal debe estar activo
        panelAjustes.SetActive(false);
        panelMenu.SetActive(true);

        // Asegúrate de que el menú de pausa completo esté oculto
        pauseMenuUI.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused){
                // Si estás en ajustes, vuelve al panel principal
                if (panelAjustes.activeSelf)
                {
                    VolverAlMenu();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }

        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Aseg�rate de reanudar el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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
