using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jarryanim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();  // A�adido para obtener la velocidad del Rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (anim != null)
        {
            // Usamos la velocidad real del Rigidbody para determinar si est� corriendo
            float speed = Mathf.Abs(body.velocity.x);  // Usamos la velocidad absoluta en X

            // Actualizamos el par�metro de la animaci�n de correr
            anim.SetFloat("Speed", speed);

            // Si no se est� moviendo, ajustamos la animaci�n a parado
            if (speed < 0.1f)
            {
                anim.SetFloat("Speed", 0f);
            }
        }
    }
}
