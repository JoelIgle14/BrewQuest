using System.Collections;
using UnityEngine;

public class CargaPresionAttack : MonoBehaviour, IBossAttack
{
    public float cargaDuration = 2f;
    public float recoveryDuration = 1.5f;
    public float triggerActiveTime = 0.3f;
    public float moveSpeed = 5f;

    private AudioController controller;
    //controller = FindObjectOfType<AudioController>();
    //controller.SeleccionAudio(6, 0.2f);


    public GameObject explosionEffectPrefab;
    public Transform[] movePoints; // 3 puntos (izquierda, centro, derecha)

    private Transform bossTransform;  // referencia ra�z para mover todo el boss
    private CircleCollider2D et;
    private Vector3 originalPosition;

    private void Awake()
    {
        // Mover el objeto ra�z para afectar todo el boss
        bossTransform = transform.root;
        et = GetComponent<CircleCollider2D>();
        controller = FindObjectOfType<AudioController>();
    }

    public IEnumerator Execute()
    {
        Debug.Log("El boss se prepara para moverse...");

        originalPosition = bossTransform.position;

        Transform target = movePoints[Random.Range(0, movePoints.Length)];

        // Moverse al punto elegido (mueve toda la ra�z del boss)
        yield return StartCoroutine(MoveToPosition(target.position));

        Debug.Log("Cargando presi�n...");
        yield return StartCoroutine(VibrateDuringCharge(cargaDuration));

        Debug.Log("�EXPLOSI�N de vapor!");
        Explode();

        yield return new WaitForSeconds(recoveryDuration);

        // Volver a la posici�n original
        yield return StartCoroutine(MoveToPosition(originalPosition));

        Debug.Log("Ataque de carga presi�n completado.");
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
        controller.SeleccionAudio(11, 0.2f);
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

        bossTransform.position = originalPos; // Restaurar posici�n
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

        public Vector3? GetDesiredPosition()
{
    return null; // Este ataque no necesita moverse antes de ejecutarse
}

}
