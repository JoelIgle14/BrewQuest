using UnityEngine;

public class CervezaPickup : MonoBehaviour
{
    public int puntos = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoints(puntos);
            }

            Destroy(gameObject);
        }
    }
}
