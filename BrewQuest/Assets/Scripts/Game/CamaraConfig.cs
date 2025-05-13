using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraConfig : MonoBehaviour
{
    public GameObject mc;  
    public Vector3 followOffset = new Vector3(3f, 2.16f, -10f);

    public PolygonCollider2D borde; 

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.orthographic = true;
            cam.orthographicSize = 6f;
        }
    }

    void LateUpdate()
    {
        if (mc != null && borde != null)
        {
            Vector3 currentCamPos = transform.position;

            // Deseado X según el personaje
            float targetX = mc.transform.position.x + followOffset.x;

            // Calculamos el Y actual y el Y del personaje + offset
            float targetY = mc.transform.position.y + followOffset.y;

            // Solo actualizamos Y si el jugador está por debajo de la cámara
            float newY = (targetY < currentCamPos.y) ? targetY : currentCamPos.y;

            // Construimos nueva posición deseada
            Vector3 targetPosition = new Vector3(targetX, newY, followOffset.z);

            // Aseguramos que está dentro del borde
            Vector2 targetPos2D = new Vector2(targetPosition.x, targetPosition.y);
            if (borde.OverlapPoint(targetPos2D))
            {
                transform.position = targetPosition;
            }
        }
    }

}
