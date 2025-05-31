using System.Collections;
using UnityEngine;

public class CargaPresionAttack : MonoBehaviour, IBossAttack
{
    public float cargaDuration = 2f;
    public float recoveryDuration = 1.5f;
    public float triggerActiveTime = 0.3f;
    public float moveSpeed = 5f;

    public GameObject explosionEffectPrefab;
    public Transform[] movePoints; // 3 puntos (izquierda, centro, derecha)

    private Transform bossTransform;  // referencia raíz para mover todo el boss
    private CircleCollider2D et;
    private Vector3 originalPosition;

    private void Awake()
    {
        // Mover el objeto raíz para afectar todo el boss
        bossTransform = transform.root;
        et = GetComponent<CircleCollider2D>();
    }

    public IEnumerator Execute()
    {
        Debug.Log("El boss se prepara para moverse...");

        originalPosition = bossTransform.position;

        Transform target = movePoints[Random.Range(0, movePoints.Length)];

        // Moverse al punto elegido (mueve toda la raíz del boss)
        yield return StartCoroutine(MoveToPosition(target.position));

        Debug.Log("Cargando presión...");
        yield return StartCoroutine(VibrateDuringCharge(cargaDuration));

        Debug.Log("¡EXPLOSIÓN de vapor!");
        Explode();

        yield return new WaitForSeconds(recoveryDuration);

        // Volver a la posición original
        yield return StartCoroutine(MoveToPosition(originalPosition));

        Debug.Log("Ataque de carga presión completado.");
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        while (Vector3.Distance(bossTransform.position, targetPos) > 0.05f)
        {
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator VibrateDuringCharge(float duration)
    {
        float elapsed = 0f;
        Vector3 originalPos = bossTransform.position;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.05f, 0.05f);
            float y = Random.Range(-0.05f, 0.05f);

            bossTransform.position = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bossTransform.position = originalPos; // Restaurar posición
    }

    private void Explode()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, bossTransform.position, Quaternion.identity);
        }

        StartCoroutine(ActivateExplosionTrigger());
    }

    private IEnumerator ActivateExplosionTrigger()
    {
        et.enabled = true;
        yield return new WaitForSeconds(triggerActiveTime);
        et.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PerderVida();
        }
    }
}
