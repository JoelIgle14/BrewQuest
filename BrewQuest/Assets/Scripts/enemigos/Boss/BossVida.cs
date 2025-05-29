using System.Collections;
using UnityEngine;

public class BossVida : MonoBehaviour
{
    public float health;
    public bool golpeado;
    public int puntos = 1000; // ← Añade puntos por este enemigo
    private BossController bc;

    Animator animator;

    void Awake()
    {
        bc = GetComponent<BossController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (bc.canTakeDamage)
        {
            TakeDamage();
        }
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
