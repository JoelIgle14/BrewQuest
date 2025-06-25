using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SoftFollowCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0, 0, -10); // Editable en X, Y, Z desde el Inspector

    public float smoothSpeed = 5f;

    [Header("Dead Zone Vertical")]
    public float verticalDeadZone = 2.5f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position + offset;

        // --- Movimiento en Y con zona muerta ---
        float deltaY = target.position.y + offset.y - camPos.y;
        if (Mathf.Abs(deltaY) > verticalDeadZone)
        {
            float direction = Mathf.Sign(deltaY);
            camPos.y = Mathf.Lerp(camPos.y, target.position.y + offset.y - (verticalDeadZone * direction), Time.deltaTime * smoothSpeed);
        }

        // --- Movimiento en X suave siempre ---
        camPos.x = Mathf.Lerp(camPos.x, targetPos.x, Time.deltaTime * smoothSpeed);

        // --- Mantener la Z como en el offset ---
        camPos.z = targetPos.z;

        transform.position = camPos;
    }
}
