using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
        {
            ResetPos();
        }
    }

    void ResetPos()
    {
        Debug.Log("Die() llamado desde GameController");
        Respawn();

    }

    void Respawn()
    {
        transform.position = startPos;
    }
}
