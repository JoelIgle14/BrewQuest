using System.Collections;
using UnityEngine;

public class CargaPresionAttack : MonoBehaviour, IBossAttack
{
    public float cargaDuration = 2f;
    public float recoveryDuration = 1.5f;
    public float triggerActiveTime = 0.3f;

    public GameObject explosionEffectPrefab;

    private Transform bossTransform;
    private CircleCollider2D et;

    private void Awake()
    {
        bossTransform = transform;
        et = GetComponent<CircleCollider2D>();
    }

    public IEnumerator Execute()
    {
        Debug.Log("El boss comienza a cargar presión...");

        // Iniciar vibración
        yield return StartCoroutine(VibrateDuringCharge(cargaDuration));

        Debug.Log("¡EXPLOSIÓN de vapor!");
        Explode();

        yield return new WaitForSeconds(recoveryDuration);
    }

    private IEnumerator VibrateDuringCharge(float duration)
    {
        float elapsed = 0f;
        Vector3 originalPos = bossTransform.parent.position;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.05f, 0.05f);
            float y = Random.Range(-0.05f, 0.05f);

            bossTransform.parent.position = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bossTransform.parent.position = originalPos; // Restaurar posición
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
