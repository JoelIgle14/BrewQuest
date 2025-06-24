using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jarryanim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D body;

    // Número de vidas (puedes enlazarlo desde otro script si lo manejas en otro lado)
    public int vidas = 3;

    void Awake()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        anim.SetInteger("Vidas", vidas);  // Inicializa el parámetro en el Animator
    }

    void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (anim != null)
        {
            // Control de movimiento
            float speed = Mathf.Abs(body.velocity.x);
            anim.SetFloat("Speed", speed);

            // Actualiza también el parámetro "Vidas" en caso de que haya cambiado
            anim.SetInteger("Vidas", vidas);
        }
    }

    // Este método se puede llamar desde otro script cuando pierdas una vida
    public void PerderVida()
    {
        if (vidas > 0)
        {
            vidas--;
            anim.SetInteger("Vidas", vidas);
            anim.SetTrigger("Dañado");  // Este trigger activa la animación de daño si la configuras
        }
    }
}