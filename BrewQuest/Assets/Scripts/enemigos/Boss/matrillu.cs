using System.Collections;
using UnityEngine;

public class matrillu : MonoBehaviour, IBossAttack
{
    public Transform target;
    public float levitationHeight = 5f;
    public float levitationDuration = 1f;
    public float pauseBeforeFall = 1f;
    public float fallSpeed = 15f;
    public float recoveryTime = 1f;

    private Vector3 originalPosition;

    private Transform bossTransform;

    private void Awake()
    {
        bossTransform = GetComponentInParent<BossController>().transform;
    }

    public IEnumerator Execute()
{
    originalPosition = bossTransform.position;

    Debug.Log("¡Martillo al ataque!");

    // Subida
    Vector3 levitationTarget = new Vector3(bossTransform.position.x, target.position.y + levitationHeight, target.position.z);
    yield return MoveToPosition(levitationTarget, levitationDuration);

    // Desplazamiento horizontal lento mientras está arriba
    float elapsed = 0f;
    while (elapsed < pauseBeforeFall)
    {
        Vector3 horizontalTarget = new Vector3(target.position.x, bossTransform.position.y, target.position.z);
        bossTransform.position = Vector3.Lerp(bossTransform.position, horizontalTarget, Time.deltaTime * 2f); // velocidad ajustable
        elapsed += Time.deltaTime;
        yield return null;
    }

    // Caída
    Vector3 fallTarget = new Vector3(bossTransform.position.x, target.position.y, target.position.z);
    yield return FallToPosition(fallTarget, fallSpeed);

    // Impacto - podrías agregar efectos aquí
    Debug.Log("¡Impacto del martillo!");

    // Espera tras caer
    yield return new WaitForSeconds(recoveryTime);

    // Regreso
    yield return MoveToPosition(originalPosition, levitationDuration);

    // Sacudir cámara
CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .2f);
// Partículas
Instantiate(impactParticles, bossTransform.position, Quaternion.identity);

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

    private IEnumerator FallToPosition(Vector3 targetPos, float speed)
    {
        while (bossTransform.position.y > targetPos.y)
        {
            bossTransform.position += Vector3.down * speed * Time.deltaTime;
            yield return null;
        }

        bossTransform.position = targetPos;
    }
}
