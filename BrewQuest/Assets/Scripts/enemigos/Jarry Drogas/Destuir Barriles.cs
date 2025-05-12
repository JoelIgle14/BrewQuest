using UnityEngine;

public class BarrilDestructible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            Destroy(gameObject);
        }
    }
}
