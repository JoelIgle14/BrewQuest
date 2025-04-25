using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraConfig : MonoBehaviour
{


    public GameObject mc;

    void Update()
    {
        Vector3 position = transform.position; // Aquí estaba el error :)
        position.x = mc.transform.position.x;
        position.y = mc.transform.position.y;
        transform.position = position;

    }

}
