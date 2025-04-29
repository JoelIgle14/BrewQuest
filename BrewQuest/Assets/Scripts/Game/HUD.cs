using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject[] vidas;
    public GameManager gameManager;

    private Animator[] animators;

    void Start()
    {
        // Inicializamos los animators de cada vida
        animators = new Animator[vidas.Length];
        for (int i = 0; i < vidas.Length; i++)
        {
            animators[i] = vidas[i].GetComponent<Animator>();
        }
    }

    // Gestion para desactivar vida
    public void DesactivarVida(int i)
    {
        if (i >= 0 && i < vidas.Length)
        {
            // En vez de SetActive false directo, primero animamos
            animators[i].Play("LoseHeart");  // Reproduce la animación de perder vida
        }
    }

    public void ActivarVida(int i)
    {
        if (i >= 0 && i < vidas.Length)
        {
            vidas[i].SetActive(true);
            animators[i].Play("GainHeart");  // Reproduce la animación de ganar vida
        }
    }

    public void VaciarVida(int i)
    {
        if (i >= 0 && i < vidas.Length)
        {
            animators[i].Play("EmptyHeart");  // Si quieres dejar la vida vacía directamente
        }
    }
}
