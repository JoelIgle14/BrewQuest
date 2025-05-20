using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int Vidas = 3;

    //recordatorio habilidades
    public bool canJump = true;
    public bool canAttack = true;
    public bool canMove = true;
    public bool hasDash = false;
    public bool hasDoubleJump = false;
    public bool hasShoot = false;


    public HUD hud;
    [SerializeField] private NewBehaviourScript habManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evitar multiples instancias del GameManager
            Debug.Log("Cuidado! Más de un GameManager en escena.");
            return;
        }

        // Suscribirse al evento de la carga de la escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Asignar las referencias cuando la nueva escena se haya cargado
        if (hud == null)
        {
            hud = FindObjectOfType<HUD>();
        }

        if (habManager == null)
        {
            habManager = FindObjectOfType<NewBehaviourScript>();
        }

        // Inicializar vidas y HUD si no es cero
        if (Vidas == 0)
        {
            Vidas = 3;
            hud.ActivarVida(Vidas);
            Debug.Log("Tienes 3 vidas");
        }
    }

    public void PerderVida()
    {
        Vidas -= 1;
        hud.DesactivarVida(Vidas);

        if (Vidas == 0)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player.respawnPoint != null)
            {
                CheckpointData.ultimaPosicionCheckpoint = player.respawnPoint.position;
            }

            // Recargar la escena
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public bool RecuperarVida()
    {
        if (Vidas == 3)
        {
            return false;
        }

        hud.ActivarVida(Vidas);
        Vidas += 1;

        return true;
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento cuando el GameManager se destruya
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
