using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerniShot : MonoBehaviour
{
    public GameObject bernishot;
    private GameObject target;
    private bernidirection berniDir; // ← referencia al script de dirección

    public float rangeDistance;
    public float fireCooldown = 1f;
    private float fireTimer = 0f;

    public float shootOffsetX = 0.5f; // separación lateral del disparo

    void Start()
    {
        target = GameObject.Find("Jarry");
        berniDir = GetComponent<bernidirection>(); // ← asumimos que está en el mismo GameObject
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);

        if (distanceToPlayer > rangeDistance && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }
    }

    private void Shoot()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        // Usa la dirección almacenada por el script de giro
        int dir = berniDir.lookDirection;
        Vector3 spawnPos = transform.position + new Vector3(shootOffsetX * dir, 0f, 0f);

        GameObject bullet = Instantiate(bernishot, spawnPos, Quaternion.identity);
        bullet.GetComponent<berniBullet>().setDirection(direction);
    }
}
