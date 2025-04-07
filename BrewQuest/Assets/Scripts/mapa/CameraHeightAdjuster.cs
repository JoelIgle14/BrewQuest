using UnityEngine;

public class CameraBorderAdjuster : MonoBehaviour
{
    public Transform grid;  // Arrastra el objeto "Grid" en el Inspector
    public Transform player; // Arrastra el personaje aqu�
    public float offsetBottom = 2f; // Distancia hacia abajo para que se vea el bloque siguiente
    public float offsetTop = 3f;    // Margen superior para que el jugador no salga de c�mara

    void Update()
    {
        if (grid == null || player == null) return;

        float maxHeight = float.MinValue;

        // Encuentra el bloque m�s alto
        foreach (Transform child in grid)
        {
            if (child.position.y > maxHeight)
            {
                maxHeight = child.position.y;
            }
        }

        // Define la nueva posici�n del borde
        Vector3 newPosition = transform.position;
        newPosition.y = maxHeight - offsetBottom; // Baja el borde para mostrar bloques debajo
        transform.position = newPosition;

        // Ajusta la c�mara para que el personaje nunca salga de la vista
        float cameraLimitY = maxHeight + offsetTop;
        if (player.position.y > cameraLimitY)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraLimitY, Camera.main.transform.position.z);
        }
    }
}
