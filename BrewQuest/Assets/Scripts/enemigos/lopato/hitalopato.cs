using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitalopato : MonoBehaviour
{
    public float golpeForceX = 3f;
    public float golpeForceY = 3f;
    public float golpeDuration = 0.5f;

    private Enemyvida ev;
    private Rigidbody2D rb;
    private PlayerMovement jarry;
    private lopatomove chase;

    public Material flashMaterial;         // ← arrástralo en el Inspector
    private Material defaultMaterial;
    private SpriteRenderer[] sprites;

    public float flashTime = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ev = GetComponent<Enemyvida>();
        jarry = FindObjectOfType<PlayerMovement>();
        chase = GetComponent<lopatomove>();

        sprites = GetComponentsInChildren<SpriteRenderer>();

        if (sprites.Length > 0)
        {
            defaultMaterial = sprites[0].material;
        }
    }

    public void EmpujeEnemigo()
    {
        StartCoroutine(GolpeCoroutine());
    }

    IEnumerator GolpeCoroutine()
    {
        // ⚪ Activar material blanco
        foreach (var sr in sprites)
        {
            sr.material = flashMaterial;
            sr.material.SetFloat("_FlashAmount", 1f);
        }

        // Empuje (Knockback)
        float direction = (jarry.transform.position.x > transform.position.x) ? -1f : 1f;
        Vector2 baseDirection = new Vector2(direction, 1f).normalized;
        Vector2 golpeVector = new Vector2(baseDirection.x * golpeForceX, baseDirection.y * golpeForceY);

        rb.velocity = Vector2.zero;
        rb.AddForce(golpeVector, ForceMode2D.Impulse);

        yield return new WaitForSeconds(flashTime);

        // 🔁 Restaurar material original
        foreach (var sr in sprites)
        {
            sr.material = defaultMaterial;
        }

        // Esperar duración del golpe
        yield return new WaitForSeconds(golpeDuration - flashTime);

        chase.isBeingHit = false;
        ev.golpeado = false;
    }
}
