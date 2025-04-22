using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] vidas;


    void Update()
    {

    }

    //Gestion para activar vidas
    public void DesactivarVida(int i)
    {
        if (i >= 0 && i < vidas.Length)
        {
            vidas[i].SetActive(false);
        }
    }


    public void ActivarVida(int i) 
    {
        vidas[i].SetActive(true);
    }
}
