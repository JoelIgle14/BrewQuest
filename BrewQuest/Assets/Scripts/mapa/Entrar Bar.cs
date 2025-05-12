using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntarBar : MonoBehaviour
{
    private bool jugadorEnZona = false;

    private void Update()
    {
        if (jugadorEnZona && Input.GetKeyDown(KeyCode.W))
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("¡Último nivel alcanzado!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Puerta"))
        {
            jugadorEnZona = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Puerta"))
        {
            jugadorEnZona = false;
        }
    }
}
