using UnityEngine;

public class CameraHeightAdjuster : MonoBehaviour
{
    public Transform[] bloques; // Asigna manualmente o encuentra todos los bloques en la escena
    public float offset = 0.1f; // 10 cm en metros

    void Update()
    {
        float maxHeight = float.MinValue;

        // Encuentra el bloque más alto
        foreach (Transform bloque in bloques)
        {
            if (bloque.position.y > maxHeight)
            {
                maxHeight = bloque.position.y;
            }
        }

        // Ajusta la posición de la cámara
        Vector3 newPosition = transform.position;
        newPosition.y = maxHeight + offset;
        transform.position = newPosition;
    }
}
