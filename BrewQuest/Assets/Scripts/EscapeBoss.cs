using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    public BossBarril bossScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossScript.EmpezarHuida(); // Cuando el jugador cruza, el jefe huye
            Destroy(gameObject); // Elimina el trigger para que no se repita
        }
    }
}
