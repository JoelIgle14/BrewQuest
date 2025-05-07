using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform respawnPoint;
    public float respawnDelay = 2f;

    private Animator anim;

    private void Start()
{
    anim = GetComponent<Animator>();

    if (CheckpointData.ultimaPosicionCheckpoint.HasValue)
    {
        transform.position = CheckpointData.ultimaPosicionCheckpoint.Value;
        CheckpointData.ultimaPosicionCheckpoint = null; // solo una vez
    }

    if (respawnPoint == null)
    {
        Debug.LogWarning("Respawn point no asignado. Asigna un punto de respawn en el Inspector.");
    }
}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallZone"))
        {
            Die();
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            UpdateRespawnPoint(collision.transform);
        }
    }

    public void Die()
    {
        GameManager.Instance.PerderVida();
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogError("No se ha asignado un punto de respawn.");
        }
    }

    void UpdateRespawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
        Debug.Log("Checkpoint actualizado: " + newRespawnPoint.position);
    }
}
