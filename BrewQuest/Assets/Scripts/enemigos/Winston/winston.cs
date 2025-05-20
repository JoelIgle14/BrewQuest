using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winston : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject bulletPrefab;
    public Transform player;
    public Transform firePoint;

    public float bulletInterval = 1f;
    public float detectionRange = 5f;

    private bool haVistoJugador = false;
    private Coroutine dispararCoroutine = null;

    private Enemyvida ev;

    void Start()
    {
        ev = GetComponent<Enemyvida>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            haVistoJugador = true;
        }

        if (haVistoJugador && !ev.golpeado && dispararCoroutine == null)
        {
            MirarAlJugador();
            dispararCoroutine = StartCoroutine(DispararCadaIntervalo());
        }

        if (ev.golpeado && dispararCoroutine != null)
        {
            StopCoroutine(dispararCoroutine);
            dispararCoroutine = null;
        }
    }

    void MirarAlJugador()
    {
        if (player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x > transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void Shoot()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<DisparoWinston>()?.SetDirection(dir);
    }

    IEnumerator DispararCadaIntervalo()
    {
        while (true)
        {
            MirarAlJugador();
            Shoot();
            yield return new WaitForSeconds(bulletInterval);
        }
    }

    public bool HaVistoJugador()
    {
        return haVistoJugador;
    }
}
