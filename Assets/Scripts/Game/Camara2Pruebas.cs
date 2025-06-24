using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -10);
    public float smoothSpeed = 5f;
    public float verticalDelay = 0.3f;

    public PolygonCollider2D borde;

    private float currentVelocityY = 0f;
    private float targetY;
    private float timer = 0f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null || borde == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Eje Y con retardo
        if (target.position.y > transform.position.y)
        {
            timer += Time.deltaTime;
            if (timer > verticalDelay)
                targetY = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref currentVelocityY, 0.3f);
            else
                targetY = transform.position.y;
        }
        else
        {
            targetY = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref currentVelocityY, 0.15f);
            timer = 0f;
        }

        Vector3 smoothedPosition = new Vector3(
            Mathf.Lerp(transform.position.x, desiredPosition.x, Time.deltaTime * smoothSpeed),
            targetY,
            Mathf.Lerp(transform.position.z, desiredPosition.z, Time.deltaTime * smoothSpeed)
        );

        Vector2 camCenter2D = new Vector2(smoothedPosition.x, smoothedPosition.y);

        // Verificamos si el nuevo centro de cámara está dentro del polígono
        if (borde.OverlapPoint(camCenter2D))
        {
            transform.position = smoothedPosition;
        }
        // Si no lo está, no movemos la cámara (se mantiene donde está)
    }
}
