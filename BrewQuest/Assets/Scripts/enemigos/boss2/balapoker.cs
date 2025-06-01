using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balapoker : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 3f;
    public Vector2 direction = Vector2.left;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

}
