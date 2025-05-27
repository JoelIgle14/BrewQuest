using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMovement move;

    //Destas de si puede usar tal habilidad
    public bool canJump = true;
    public bool canAttack = true;
    public bool canMove = true;
    public bool canDash = false;
    public bool canDoubleJump = false;
    public bool canShoot = false;

    void Awake()
    {
        move = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        // Al reaparecer, consultar GameManager
        if (GameManager.Instance != null)
        {
            canJump = GameManager.Instance.canJump;
            canAttack = GameManager.Instance.canAttack;
            canMove = GameManager.Instance.canMove;
            canDash = GameManager.Instance.hasDash;
            canDoubleJump = GameManager.Instance.hasDoubleJump;
            canShoot = GameManager.Instance.hasShoot;

            // Si puede disparar, activar el power up
            if (canShoot)
                GetComponent<Disparo>().ActivarPowerUp();
        }
    }
}
