using System.Collections;
using System.Collections.Generic;
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

    public void TakeDamage(float amount)
    {
        if (!golpeado) // Solo hace algo si no está siendo golpeado ya
        {
            golpeado = true;
            Debug.Log("pene");
            health -= amount;

            if (health <= 0)
            {
                Destroy(gameObject); // o tu lógica de muerte
                // añadir animación de muerte aquí si es necesario
            }

            animator.SetTrigger("hit");
            Debug.Log("dick");

            // Opcional: Hacer que el golpe dure un tiempo limitado y después resetear golpeado
            StartCoroutine(ResetGolpeado());
        }
    }

    // Coroutine que restablece el estado de golpeado después de un tiempo
    private IEnumerator ResetGolpeado()
    {
        yield return new WaitForSeconds(1f); // Este tiempo debe coincidir con la duración de tu golpe
        golpeado = false; // Restablece el estado de golpeado
    }
}
