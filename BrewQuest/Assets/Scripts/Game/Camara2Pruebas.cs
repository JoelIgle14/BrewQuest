using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El personaje a seguir
    public Vector3 offset = new Vector3(0, 3, -10); // Posición relativa a CJ
    public float smoothSpeed = 5f; // Suavizado general
    public float verticalDelay = 0.3f; // Tiempo de retardo al seguir salto

    private float currentVelocityY = 0f;
    private float targetY;
    private float timer = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Solo actualizamos el eje Y con retardo
        if (target.position.y > transform.position.y)
        {
            // El personaje está saltando: esperamos un poco
            timer += Time.deltaTime;
            if (timer > verticalDelay)
                targetY = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref currentVelocityY, 0.3f);
            else
                targetY = transform.position.y; // aún no seguimos el salto
        }
        else
        {
            // El personaje cae o se mantiene: seguimos inmediatamente
            targetY = Mathf.SmoothDamp(transform.position.y, desiredPosition.y, ref currentVelocityY, 0.15f);
            timer = 0f;
        }

        Vector3 finalPosition = new Vector3(
            Mathf.Lerp(transform.position.x, desiredPosition.x, Time.deltaTime * smoothSpeed),
            targetY,
            Mathf.Lerp(transform.position.z, desiredPosition.z, Time.deltaTime * smoothSpeed)
        );

        transform.position = finalPosition;
    }
}
