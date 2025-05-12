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
            Vector3 targetPosition = mc.transform.position + followOffset;

            
            Vector2 targetPos2D = new Vector2(targetPosition.x, targetPosition.y);

            
            if (borde.OverlapPoint(targetPos2D))
            {
                
                transform.position = targetPosition;
            }
            else
            {
               
            }
        }
    }
}
