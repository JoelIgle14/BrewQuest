using System.Collections;
using UnityEngine;

public class matrillu : MonoBehaviour, IBossAttack
{
    [Header("Referencias")]
    public Transform target;
    public GameObject shadowPrefab;
    public CameraShake cameraShake;

    [Header("Tiempos y alturas")]
    public float levitationHeight = 7f; // Más alto para acentuar caída
    public float levitationDuration = 1f;
    public float pauseBeforeFall = 1f;
    public float recoveryTime = 1f;

    [Header("Caída")]
    public float initialFallSpeed = 0f;
    public float fallAcceleration = 80f;

    [Header("Cámara Shake")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 1.0f;


    private Vector3 originalPosition;
    private Transform bossTransform;
    //private GameObject shadowInstance;

    private void Awake()
    {
        bossTransform = GetComponentInParent<BossController>().transform;
    }

    public IEnumerator Execute()
    {
        originalPosition = bossTransform.position;
        Debug.Log("¡Martillo al ataque!");

        // Subir a la posición de levitación
        Vector3 levitationTarget = new Vector3(
            bossTransform.position.x,
            target.position.y + levitationHeight,
            target.position.z
        );
        yield return MoveToPosition(levitationTarget, levitationDuration);

        // Crear sombra predictiva
        //Vector3 shadowPos = new Vector3(target.position.x, target.position.y + 0.01f, target.position.z);
        //shadowInstance = Instantiate(shadowPrefab, shadowPos, Quaternion.identity);

        // Movimiento horizontal hacia el objetivo mientras espera
        float elapsed = 0f;
        while (elapsed < pauseBeforeFall)
        {
            Vector3 horizontalTarget = new Vector3(target.position.x, bossTransform.position.y, target.position.z);
            bossTransform.position = Vector3.Lerp(bossTransform.position, horizontalTarget, Time.deltaTime * 2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Caída acelerada
        yield return FallToPosition(target.position.y);

        // Impacto
        Debug.Log("¡Impacto del martillo!");

        if (cameraShake != null){
            yield return cameraShake.Shake(shakeDuration, shakeMagnitude);
        }
        //if (shadowInstance != null)
        //    Destroy(shadowInstance);

        // Espera tras el golpe
        yield return new WaitForSeconds(recoveryTime);

        // Vuelve a la posición original
        yield return MoveToPosition(originalPosition, levitationDuration);
    }

    private IEnumerator MoveToPosition(Vector3 targetPos, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = bossTransform.position;

        while (elapsed < duration)
        {
            bossTransform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossTransform.position = targetPos;
    }

    private IEnumerator FallToPosition(float targetY)
    {
        float velocity = initialFallSpeed;
        while (bossTransform.position.y > targetY)
        {
            velocity += fallAcceleration * Time.deltaTime;
            bossTransform.position += Vector3.down * velocity * Time.deltaTime;
            yield return null;
        }

        bossTransform.position = new Vector3(bossTransform.position.x, targetY, bossTransform.position.z);
    }
}
