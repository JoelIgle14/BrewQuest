using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement move;

    //Destas de si puede usar tal habilidad
    public bool canDash = false;
    public bool canDoubleJump = false;

    void Awake()
    {
        move = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Redonda"))
        {
            //conseguir tal habilidad
            Debug.Log("Rabo de Toro");
            canDash = true;
        }

        else if (other.gameObject.CompareTag("cuadrao"))
        {
            //conseguir tal habilidad
            Debug.Log("Rabo de Tora");
            canDoubleJump = true;
        }
    }
}
