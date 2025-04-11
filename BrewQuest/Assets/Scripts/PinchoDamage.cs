using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchoDamage : MonoBehaviour
{
    private PlayerMovement jarry;
    private void Awake()
    {
        jarry = GetComponent<PlayerMovement>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jarry = other.GetComponent<PlayerMovement>();

            if (jarry != null && GameManager.Instance != null)
            {
                GameManager.Instance.PerderVida();
                float direction = (other.transform.position.x > jarry.transform.position.x) ? -1 : 1;
                jarry.ApplyKnockback(direction, new Vector2(5f, 7f), 0.56f);
            }
        }
    }

    IEnumerator DisableMovementForTime(float time)
    {
        jarry.canMove = false;
        yield return new WaitForSeconds(time);
        jarry.canMove = true;
    }
}
