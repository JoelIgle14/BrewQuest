using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vida : MonoBehaviour
{
    public int puntosPorVidaExtra = 10; // Puedes cambiarlo desde el Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool vidaRecuperada = GameManager.Instance.RecuperarVida();

            if (vidaRecuperada)
            {
                Destroy(this.gameObject);
            }
            else
            {
                // Dar puntos si no se puede recuperar más vida
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(puntosPorVidaExtra);
                }

                Destroy(this.gameObject);
            }
        }
    }
}
