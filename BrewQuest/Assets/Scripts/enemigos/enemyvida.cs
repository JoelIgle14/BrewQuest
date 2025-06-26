using System.Collections;
using UnityEngine;

public class Enemyvida : MonoBehaviour
{
    public float health;
    public bool golpeado;
    public int puntos = 10; // Añade puntos por este enemigo

    Animator animator;
    
    private AudioController controller;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = FindObjectOfType<AudioController>();
    }

    public void TakeDamage(float amount, GameObject Player, bool esAtaqueCuerpoACuerpo)
    {
        if (!golpeado)
        {
            controller.SeleccionAudio(6, 0.2f);
            golpeado = true;
            health -= amount;

            if (esAtaqueCuerpoACuerpo)
            {
                Disparo disparo = Player.GetComponent<Disparo>();
                if (disparo != null)
                {
                    disparo.RecargarTiro();
                }
            }

            if (health <= 0)
            {
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(puntos);
                }

                Destroy(gameObject);
                return;
            }

            animator.SetTrigger("hit");
            StartCoroutine(ResetGolpeado());
        }
    }

    private IEnumerator ResetGolpeado()
    {
        yield return new WaitForSeconds(1f);
        golpeado = false;
    }
}
