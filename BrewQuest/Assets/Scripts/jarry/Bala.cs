
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
    }
}
