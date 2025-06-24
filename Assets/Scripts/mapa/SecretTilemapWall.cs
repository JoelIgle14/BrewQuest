using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretTilemapWall : MonoBehaviour
{
    public Tilemap outsideTilemap;  // Tilemap exterior (opaco)
    public Tilemap insideTilemap;   // Tilemap interior (transparente)
    public float revealOpacity = 0.3f; // Transparencia al estar dentro

    void Start()
    {
        insideTilemap.gameObject.SetActive(false); // Oculta la pared interior al inicio
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            insideTilemap.gameObject.SetActive(true); // Activa la pared interior
            outsideTilemap.color = new Color(1f, 1f, 1f, revealOpacity); // Hace la pared exterior medio transparente
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            insideTilemap.gameObject.SetActive(false); // Oculta la pared interior
            outsideTilemap.color = new Color(1f, 1f, 1f, 1f); // Vuelve la pared exterior opaca
        }
    }
}
