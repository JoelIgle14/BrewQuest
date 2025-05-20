using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ConfinedCameraWithOffset : MonoBehaviour
{
    public Transform target;
    public PolygonCollider2D confiner;
    public Vector3 offset = new Vector3(10f, 0f, -10f);
    public float smoothSpeed = 10f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (!cam.orthographic)
        {
            Debug.LogError("La cámara debe estar en modo ortográfico.");
        }

        if (confiner == null)
        {
            Debug.LogError("Asigna un PolygonCollider2D como confiner.");
        }
    }

    void LateUpdate()
    {


        if (target == null || confiner == null) return;

        // 1. Calculamos la posición deseada de la cámara con offset
        Vector3 desiredPosition = target.position + offset;

        // 2. Calculamos tamaño visible de la cámara
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        // 3. Calculamos los 4 vértices del área visible
        Vector2[] corners = new Vector2[]
        {
            new Vector2(desiredPosition.x - horzExtent, desiredPosition.y + vertExtent), // Top Left
            new Vector2(desiredPosition.x + horzExtent, desiredPosition.y + vertExtent), // Top Right
            new Vector2(desiredPosition.x - horzExtent, desiredPosition.y - vertExtent), // Bottom Left
            new Vector2(desiredPosition.x + horzExtent, desiredPosition.y - vertExtent)  // Bottom Right
        };

        bool isInside = true;
        foreach (Vector2 corner in corners)
        {
            if (!confiner.OverlapPoint(corner))
            {
                isInside = false;
                break;
            }
        }

        Vector3 finalPosition;

        if (isInside)
        {
            // Todo el área visible cabe dentro del confiner
            finalPosition = desiredPosition;
        }
        else
        {
            // La cámara se moverá hacia la posición más cercana que esté completamente dentro
            finalPosition = FindValidCameraPosition(horzExtent, vertExtent);
        }

        finalPosition = desiredPosition;
        // Aplicamos suavizado
        transform.position = Vector3.Lerp(transform.position, finalPosition, smoothSpeed * Time.deltaTime);
    }

    Vector3 FindValidCameraPosition(float horzExtent, float vertExtent)
    {
        // Punto base: el jugador sin offset
        Vector3 basePosition = target.position;
        Vector3 bestPosition = transform.position;
        float bestDistance = float.MaxValue;

        // Vamos a muestrear varias posiciones alrededor del jugador
        for (float x = -offset.x; x <= offset.x; x += 0.5f)
        {
            for (float y = -5f; y <= 5f; y += 0.5f)
            {
                Vector3 testPos = basePosition + new Vector3(x, y, offset.z);

                Vector2[] corners = new Vector2[]
                {
                    new Vector2(testPos.x - horzExtent, testPos.y + vertExtent),
                    new Vector2(testPos.x + horzExtent, testPos.y + vertExtent),
                    new Vector2(testPos.x - horzExtent, testPos.y - vertExtent),
                    new Vector2(testPos.x + horzExtent, testPos.y - vertExtent)
                };

                bool inside = true;
                foreach (var c in corners)
                {
                    if (!confiner.OverlapPoint(c))
                    {
                        inside = false;
                        break;
                    }
                }

                if (inside)
                {
                    float dist = Vector2.Distance(testPos, basePosition + offset);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestPosition = testPos;
                    }
                }
            }
        }

        return bestPosition;
    }
}
