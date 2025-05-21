using System.Collections;
using UnityEngine;

public class Barrida : MonoBehaviour, IBossAttack
{
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public float chargeTime = 1f;
    public float dashSpeed = 10f;
    public float recoveryTime = 1f;

    private Transform bossTransform;
    private Vector3 originalPosition;
    private bool goingRight;

    private void Awake()
    {
        //bossTransform = transform;
        bossTransform = GetComponentInParent<BossController>().transform;
    }

    public IEnumerator Execute()
    {
        originalPosition = bossTransform.position;

        // 1. Elegir lado aleatorio
        goingRight = Random.value > 0.5f;
        Transform startPoint = goingRight ? spawnPointLeft : spawnPointRight;
        Transform endPoint = goingRight ? spawnPointRight : spawnPointLeft;

        // 2. Teletransporte al punto inicial
        bossTransform.position = startPoint.position;

        // 3. Espera de carga
        yield return new WaitForSeconds(chargeTime);

        // 4. Movimiento barrido
        yield return StartCoroutine(SweepAcross(startPoint.position, endPoint.position));

        // 5. Espera post-barrida
        yield return new WaitForSeconds(recoveryTime);

        // 6. Volver a la posición inicial
        bossTransform.position = originalPosition;

        Debug.Log("Barridaaaaa");
    }

    private IEnumerator SweepAcross(Vector3 start, Vector3 end)
    {
        float elapsed = 0f;
        float totalDistance = Vector3.Distance(start, end);
        float duration = totalDistance / dashSpeed;

        while (elapsed < duration)
        {
            bossTransform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossTransform.position = end;
    }
}
