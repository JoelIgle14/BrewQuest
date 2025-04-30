using UnityEngine;

public class Bala : MonoBehaviour
{
    public float daño = 1f;

    void Start()
    {
        Destroy(gameObject, 4f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión con: " + other.name);

        if (other.CompareTag("enemy"))
        {
            Debug.Log("¡Golpeó al enemigo!");

            Enemyvida enemigo = other.GetComponent<Enemyvida>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(daño);
            }

            Destroy(gameObject);
        }
    }

}
