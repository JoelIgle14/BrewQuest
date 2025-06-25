using UnityEngine;

public class CervezaPickup : MonoBehaviour
{
    public int puntos = 5;

    Animator animator;
    
    private AudioController controller;

    private void Setup()
    {
        controller = FindObjectOfType<AudioController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            controller.SeleccionAudio(6, 0.2f);

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(puntos);
            }

            Destroy(gameObject);
        }
    }
}
