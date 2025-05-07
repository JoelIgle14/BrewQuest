using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    public GameObject boss; // arrastra el boss aquí
    public BossBarril bossScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true); // Activa el GameObject del boss
            bossScript.EmpezarAtaque(); // Inicia la lógica de barriles
            Destroy(gameObject); // Elimina el trigger para que no se active más de una vez
        }
    }
}
