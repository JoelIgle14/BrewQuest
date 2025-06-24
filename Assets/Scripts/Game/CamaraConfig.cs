using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SoftFollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 5f;

    [Header("Dead Zone Vertical")]
    public float verticalDeadZone = 2.5f; // Cuánto puede alejarse en Y antes de mover la cámara

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position + offset;

        // Mantener la cámara dentro de la zona muerta en Y
        float deltaY = target.position.y - camPos.y;
        if (Mathf.Abs(deltaY) > verticalDeadZone)
        {
            float direction = Mathf.Sign(deltaY);
            camPos.y = Mathf.Lerp(camPos.y, target.position.y - (verticalDeadZone * direction), Time.deltaTime * smoothSpeed);
        }

        // Movimiento en X suave siempre
        camPos.x = Mathf.Lerp(camPos.x, targetPos.x, Time.deltaTime * smoothSpeed);

        // Mantener la Z fija
        camPos.z = offset.z;

        transform.position = camPos;
    }
}
