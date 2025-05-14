using UnityEngine;

public class FallingPlatform : MonoBehaviour, IReiniciable
{
    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isShaking = false;

    public float fallDelay = 0.5f;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float shakeSpeed = 50f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isShaking)
        {
            StartCoroutine(ShakeThenFall());
        }
    }

    private System.Collections.IEnumerator ShakeThenFall()
    {
        isShaking = true;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude;
            transform.position = originalPosition + new Vector3(xOffset, 0f, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;

        yield return new WaitForSeconds(fallDelay);

        rb.isKinematic = false;
    }

    public void Reiniciar()
    {
        StopAllCoroutines();
        isShaking = false;
        transform.position = originalPosition;
        transform.rotation = originalRotation; // << NUEVO
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }
}
