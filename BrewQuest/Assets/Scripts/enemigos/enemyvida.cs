using System.Collections;
using UnityEngine;

public class Enemyvida : MonoBehaviour
{
    public float health;
    public bool golpeado;
    public int puntos = 10; // ← Añade puntos por este enemigo

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount, GameObject Player, bool esAtaqueCuerpoACuerpo)
    {
        if (!golpeado)
        {
            golpeado = true;
            health -= amount;

            if (health <= 0)
            {
                // Sumar puntos antes de destruir
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(puntos);
                }

                Destroy(gameObject);
                return;
            }

            animator.SetTrigger("hit");

            if (esAtaqueCuerpoACuerpo)
            {
                Disparo disparo = Player.GetComponent<Disparo>();
                if (disparo != null)
                {
                    disparo.RecargarTiro();
                }
            }

            StartCoroutine(ResetGolpeado());
        }
    }

    private IEnumerator ResetGolpeado()
    {
        yield return new WaitForSeconds(1f);
        golpeado = false;
    }
}
