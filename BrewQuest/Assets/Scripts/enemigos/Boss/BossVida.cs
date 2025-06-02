using System.Collections;
using UnityEngine;

public class BossVida : MonoBehaviour
{
    public float health;
    public bool golpeado;
    public int puntos = 1000; 
    private BossController bc;
    public GameObject player;

    void Awake()
    {
        bc = GetComponent<BossController>();
    }

    public void TakeDamage(float amount, GameObject Player, bool esAtaqueCuerpoACuerpo)
    {
        if (!golpeado && bc != null && bc.canTakeDamage)
        {
            bc.FlashOnHit();
            golpeado = true;
            health -= amount;
            //a

            if (health <= 0)
            {
                // Sumar puntos antes de destruir
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(puntos);
                }

                Destroy(gameObject);
                ActivateDoor();
            }

            //animator.SetTrigger("hit");

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

    private void ActivateDoor()
    {
        GameObject puerta = GameObject.Find("Puerta2");
        if (puerta != null)
        {
            //puerta.SetActive(true);
            puerta.GetComponent<SpriteRenderer>().enabled = true;
            puerta.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            Debug.LogWarning("No puerta jeje");
        }
    }


    private IEnumerator ResetGolpeado()
    {
        yield return new WaitForSeconds(1f);
        golpeado = false;
    }
}
