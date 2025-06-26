using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject[] vidas;
    public GameManager gameManager;

    private Animator[] animators;

    private int vidasActivas; // Ya no inicializamos aquí

    void Start()
    {
        // Inicializamos los animators de cada vida
        animators = new Animator[vidas.Length];
        for (int i = 0; i < vidas.Length; i++)
        {
            animators[i] = vidas[i].GetComponent<Animator>();
        }

        // Obtener la cantidad actual de vidas desde el GameManager
        vidasActivas = GameManager.Instance.GetVidas();
        SincronizarTodosLosCorazones();
    }

    public void DesactivarVida(int i)
    {
        if (i >= 0 && i < vidas.Length)
        {
            animators[i].Play("LoseHeart");
            vidasActivas = Mathf.Max(0, vidasActivas - 1);
        }
    }

    public void ActivarVida(int i)
    {
        if (i >= 0 && i < vidas.Length)
        {
            vidas[i].SetActive(true);
            animators[i].Play("GainHeart");
            vidasActivas = Mathf.Min(vidas.Length, vidasActivas + 1);
            StartCoroutine(ActivarYSincronizar(i));
        }
    }

    private IEnumerator ActivarYSincronizar(int i)
    {
        yield return null; // esperar un frame

        float duracion = animators[i].GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracion);

        SincronizarTodosLosCorazones(); // sincroniza todos después de ganar vida
    }

    private IEnumerator SincronizarIdleDespuesDeAnimacion(Animator animator, string animActual, string animIdle)
    {
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float duracion = stateInfo.length;

        yield return new WaitForSeconds(duracion);

        animator.Play(animIdle, 0, 0f);
    }

    public void SincronizarTodosLosCorazones()
    {
        // Ahora siempre leemos las vidas actuales del GameManager
        vidasActivas = GameManager.Instance.GetVidas();

        for (int i = 0; i < vidas.Length; i++)
        {
            if (i < vidasActivas)
            {
                vidas[i].SetActive(true);
                animators[i].Play("IdleHeart", 0, 0f);
            }
            else
            {
                vidas[i].SetActive(true);
                animators[i].Play("EmptyHeart", 0, 0f);
            }
        }
    }

    public void VaciarVida(int i)
    {
        if (i >= 0 && i < vidas.Length)
        {
            animators[i].Play("EmptyHeart");
        }
    }
}
