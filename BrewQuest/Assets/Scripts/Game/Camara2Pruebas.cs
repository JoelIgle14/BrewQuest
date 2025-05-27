using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowPoint : MonoBehaviour
{
    [Header("Objetos de seguimiento")]
    public Transform followPoint; // Punto debajo del personaje (por ejemplo, un hijo del jugador)

    [Header("Ajustes de cámara")]
    public Vector2 offset = new Vector2(3f, 2f); // Offset relativo (adelante y arriba)
    public float followSpeed = 5f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (!cam.orthographic)
            Debug.LogWarning("Se recomienda que la cámara sea ortográfica para juegos 2D.");

        if (followPoint == null)
            Debug.LogError("Asigna el Transform del punto de seguimiento.");
    }

    void LateUpdate()
    {
        if (followPoint == null) return;

        // Dirección horizontal según la escala (flip del personaje)
        float dir = Mathf.Sign(followPoint.localScale.x);

        // Posición objetivo
        Vector3 targetPosition = followPoint.position + new Vector3(offset.x * dir, offset.y, -10f);

        // Movimiento suavizado
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
