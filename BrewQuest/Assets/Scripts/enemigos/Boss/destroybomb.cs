using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroybomb : MonoBehaviour
{
    public float duration = 1f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
