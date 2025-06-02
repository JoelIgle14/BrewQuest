using System.Collections;
using UnityEngine;

public class dadosvida : MonoBehaviour
{
    public float health;
    public bool golpeado = false;
    public int puntos;

    private Animator animator;
    public bool estaMuriendo = false;
    private CircleCollider2D explosionCollider;

    void Awake()
    {
        animator = GetComponent<Animator>();

        // Buscar el hijo llamado "explosion" y obtener su collider
        Transform explosion = transform.Find("explosion");
        if (explosion != null)
        {
            explosionCollider = explosion.GetComponent<CircleCollider2D>();
            if (explosionCollider != null)
                explosionCollider.enabled = false; // Desactivar por defecto
        }

        // Iniciar la cuenta atrás automática
        StartCoroutine(AutoDestruirTrasTiempo(10f));
    }

    private IEnumerator AutoDestruirTrasTiempo(float segundos)
    {
        yield return new WaitForSeconds(segundos);

        // Solo si sigue vivo
        if (!estaMuriendo)
        {
            Debug.Log("Destrucción automática tras 10 segundos.");
            StartCoroutine(MorirConExplosion());
        }
    }

    public void TakeDamage(float amount, GameObject Player, bool esAtaqueCuerpoACuerpo)
    {
        if (!golpeado && !estaMuriendo)
        {
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
                Debug.Log("¡Enemigo muerto!");
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(puntos);
                }

                StartCoroutine(MorirConExplosion());
            }
            else
            {
                animator.SetTrigger("hit");
                StartCoroutine(ResetGolpeado());
            }
        }
    }

    private IEnumerator ResetGolpeado()
    {
        yield return new WaitForSeconds(1f);
        golpeado = false;
    }

    public IEnumerator MorirConExplosion()
    {
        estaMuriendo = true;
        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("hit");

        if (explosionCollider != null)
        {
            explosionCollider.enabled = true;
            Debug.Log("Collider activado");
        }

        yield return new WaitForSeconds(0.33f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !estaMuriendo)
        {
            animator.SetTrigger("hit");
            StartCoroutine(DieWplayer());
        }
    }

    private IEnumerator DieWplayer()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
