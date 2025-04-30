using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyhab : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0.5f);
            Debug.Log("Negrata");
        }
    }
}
