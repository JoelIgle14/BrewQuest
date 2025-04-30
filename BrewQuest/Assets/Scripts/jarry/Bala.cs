using UnityEngine;

public class Bala : MonoBehaviour
{
    public float da�o = 1f;

    void Start()
    {
        Destroy(gameObject, 4f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisi�n con: " + other.name);

        if (other.CompareTag("enemy"))
        {
            Debug.Log("�Golpe� al enemigo!");

            Enemyvida enemigo = other.GetComponent<Enemyvida>();
            if (enemigo != null)
            {
                enemigo.TakeDamage(da�o);
            }

            Destroy(gameObject);
        }
    }

}
