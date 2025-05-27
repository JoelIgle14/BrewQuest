using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmartCameraFollow : MonoBehaviour
{
    public Transform followPoint;             // El punto justo debajo del jugador (no el jugador directamente)
    public Rigidbody2D playerRb;              // Rigidbody2D del jugador
    public PolygonCollider2D confiner;        // Límite del mapa

    public Vector2 offset = new Vector2(3f, 2f); // Offset desde el punto hacia adelante y arriba
    public float smoothSpeed = 5f;

    private Camera cam;
    private float lastGroundedY;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (!cam.orthographic)
            Debug.LogError("La cámara debe estar en modo ortográfico.");
        if (followPoint == null)
            Debug.LogError("Asigna el punto de seguimiento.");
        if (playerRb == null)
            Debug.LogError("Asigna el Rigidbody2D del jugador.");
        if (confiner == null)
            Debug.LogError("Asigna el PolygonCollider2D como confiner.");

        lastGroundedY = followPoint.position.y + offset.y;
    }

    void LateUpdate()
    {
        if (followPoint == null || confiner == null) return;

        // Dirección horizontal según la escala (puedes cambiar esto si prefieres velocidad.x)
        float dir = Mathf.Sign(followPoint.localScale.x);

        // Posición deseada desde el punto debajo del jugador
        Vector3 desiredPosition = followPoint.position + new Vector3(offset.x * dir, offset.y, -10f);

        // Mantener altura si está en el aire
        if (!IsGrounded())
        {
            desiredPosition.y = lastGroundedY;
        }
        else
        {
            lastGroundedY = desiredPosition.y;
        }

        // Mantener dentro del confiner
        desiredPosition = ClampCameraInsideConfiner(desiredPosition);

        // Movimiento suavizado
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Mathf.Abs(playerRb.velocity.y) < 0.01f;
    }

    Vector3 ClampCameraInsideConfiner(Vector3 desiredPos)
    {
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        Vector2[] corners = new Vector2[]
        {
            new Vector2(desiredPos.x - horzExtent, desiredPos.y + vertExtent),
            new Vector2(desiredPos.x + horzExtent, desiredPos.y + vertExtent),
            new Vector2(desiredPos.x - horzExtent, desiredPos.y - vertExtent),
            new Vector2(desiredPos.x + horzExtent, desiredPos.y - vertExtent)
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
            return desiredPos;

        // Buscar la posición más cercana válida
        Vector3 bestPosition = transform.position;
        float minDist = float.MaxValue;

        for (float dx = -1f; dx <= 1f; dx += 0.5f)
        {
            for (float dy = -1f; dy <= 1f; dy += 0.5f)
            {
                Vector3 testPos = desiredPos + new Vector3(dx, dy, 0f);

                Vector2[] testCorners = new Vector2[]
                {
                    new Vector2(testPos.x - horzExtent, testPos.y + vertExtent),
                    new Vector2(testPos.x + horzExtent, testPos.y + vertExtent),
                    new Vector2(testPos.x - horzExtent, testPos.y - vertExtent),
                    new Vector2(testPos.x + horzExtent, testPos.y - vertExtent)
                };

                bool allInside = true;
                foreach (var c in testCorners)
                {
                    if (!confiner.OverlapPoint(c))
                    {
                        allInside = false;
                        break;
                    }
                }

                if (allInside)
                {
                    float dist = Vector2.Distance(new Vector2(testPos.x, testPos.y), new Vector2(desiredPos.x, desiredPos.y));
                    if (dist < minDist)
                    {
                        minDist = dist;
                        bestPosition = testPos;
                    }
                }
            }
        }

        bestPosition.z = desiredPos.z;
        return bestPosition;
    }
}
