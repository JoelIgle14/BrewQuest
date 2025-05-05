using System.Collections;
using UnityEngine;

public class Enemyvida : MonoBehaviour
{
    public float health;
    public bool golpeado;
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
                Destroy(gameObject);
            }

            animator.SetTrigger("hit");

            // Solo recargar si el daño vino de un ataque cuerpo a cuerpo
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