using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public HUD hud;
    private int Vidas = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Cuidado! Mas de un GameManager en escena.");
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
}
