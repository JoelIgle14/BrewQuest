using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject[] vidas;
    public GameManager gameManager;

    private Animator[] animators;

    private int vidasActivas = 3; // o la cantidad con la que empieces


    void Start()
    {
        // Inicializamos los animators de cada vida
        animators = new Animator[vidas.Length];
        for (int i = 0; i < vidas.Length; i++)
        {
            animators[i] = vidas[i].GetComponent<Animator>();
        }
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
            vidasActivas = Mathf.Min(vidas.Length, vidasActivas + 1); // no pasarse de 3
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
        // Esperar 1 frame para asegurar que GainHeart realmente haya comenzado
        yield return null;

        // Obtener la duración real de la animación actual
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float duracion = stateInfo.length;

        // Esperar la duración real
        yield return new WaitForSeconds(duracion);

        // Forzar IdleHeart sincronizado
        animator.Play(animIdle, 0, 0f);
    }

    public void SincronizarTodosLosCorazones()
    {
        for (int i = 0; i < vidas.Length; i++)
        {
            if (i < vidasActivas)
            {
                // Corazón activo
                vidas[i].SetActive(true);
                animators[i].Play("IdleHeart", 0, 0f);
            }
            else
            {
                // Corazón vacío
                vidas[i].SetActive(true); // Asegurarse que está visible
                animators[i].Play("EmptyHeart", 0, 0f);
            }
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