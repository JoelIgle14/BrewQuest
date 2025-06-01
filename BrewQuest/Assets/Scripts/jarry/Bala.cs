
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float damage = 1f;
    public GameObject dueño; //  El jugador que disparó

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemyvida enemigo = other.GetComponent<Enemyvida>();
        if (enemigo != null)
        {
            enemigo.TakeDamage(damage, dueño,false); //  Aquí pasas el jugador
            Destroy(gameObject);
        }

        BossVida enemig = other.GetComponent<BossVida>();
        if (enemig != null)
        {
            enemig.TakeDamage(damage, dueño, false); //  Aquí pasas el jugador
            Destroy(gameObject);
        }

        Boss2vida enemi = other.GetComponent<Boss2vida>();
        if (enemi != null)
        {
            enemi.TakeDamage(damage, dueño, false); //  Aquí pasas el jugador
            Destroy(gameObject);
        }

        dadosvida enem = other.GetComponent<dadosvida>();
        if (enem != null)
        {
            enem.TakeDamage(damage, dueño, false); //  Aquí pasas el jugador
            Destroy(gameObject);
        }
    }
}
