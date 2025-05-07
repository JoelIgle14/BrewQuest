
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float damage = 1f;
    public GameObject due�o; //  El jugador que dispar�

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemyvida enemigo = other.GetComponent<Enemyvida>();
        if (enemigo != null)
        {
            enemigo.TakeDamage(damage, due�o,false); //  Aqu� pasas el jugador
            Destroy(gameObject);
        }
    }
}
