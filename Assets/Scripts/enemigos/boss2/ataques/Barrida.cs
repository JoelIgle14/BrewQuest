using System.Collections;
using UnityEngine;

public class Barrida : MonoBehaviour, Iboss2atac
{
    [Header("Puntos de barrida")]
    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    [Header("Par�metros de la barrida")]
    public float chargeTime = 1f;
    public float dashSpeed = 10f;
    public float recoveryTime = 1f;

    [Header("Altura fija para barrida (auto-posici�n de puntos)")]
    public float sweepY = -2.5f;

    [Header("Par�metros del hueco")]
    public float gapWidth = 2f;

    [Header("Velocidad progresiva de barrida")]
    public float sweepStartSpeed = 2f;   // velocidad inicial (alerta)
    public float sweepMaxSpeed = 12f;    // velocidad final (aceleraci�n)
    public float sweepAccelerationTime = 0.5f; // tiempo para alcanzar velocidad m�xima


    [Tooltip("Centro del hueco cuando va hacia la derecha")]
    public float gapPositionRight = 0f;

    [Tooltip("Centro del hueco cuando va hacia la izquierda")]
    public float gapPositionLeft = 0f;

    private Transform bossTransform;
    private Vector3 originalPosition;
    private bool goingRight;

    private void Awake()
    {
        bossTransform = transform.root;
    }

    public IEnumerator Execute()
    {
        originalPosition = bossTransform.position;

        // Elegir aleatoriamente la direcci�n de la barrida
        // TEST TEMPORAL para verificar que funcione
        goingRight = Random.Range(0, 2) == 0; // 50% igual, pero m�s expl�cito
                                              // goingRight = false; // <-- Fuerza barrida desde la derecha (para probar izquierda)

        // Cambiar la escala para que mire en la direcci�n del movimiento
        Vector3 scale = bossTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * (goingRight ? -1 : 1);
        bossTransform.localScale = scale;


        // Calcular extremos de la c�mara
        float zDistance = Mathf.Abs(Camera.main.transform.position.z - bossTransform.position.z);
        Vector3 leftWorld = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, zDistance));
        Vector3 rightWorld = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, zDistance));

        float spawnX, startX, endX;

        if (goingRight)
        {

            spawnX = leftWorld.x - 2f;
            startX = leftWorld.x + 1f;
            endX = rightWorld.x - 1f;
        }
        else
        {
            spawnX = rightWorld.x + 2f;
            startX = rightWorld.x - 1f;
            endX = leftWorld.x + 1f;
        }

        Vector3 flyInStart = new Vector3(spawnX, originalPosition.y, 0);
        Vector3 flyInTarget = new Vector3(startX, originalPosition.y, 0);
        Vector3 descendTarget = new Vector3(startX, sweepY, 0);
        Vector3 finalSweepPoint = new Vector3(endX, sweepY, 0);

        // Posicionar fuera de c�mara
        bossTransform.position = flyInStart;

        // Entrar volando
        while (Vector3.Distance(bossTransform.position, flyInTarget) > 0.1f)
        {
            bossTransform.position = Vector3.MoveTowards(
                bossTransform.position,
                flyInTarget,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }

        yield return new WaitForSeconds(chargeTime);

        // Bajar en vertical
        while (bossTransform.position.y > sweepY)
        {
            bossTransform.position = Vector3.MoveTowards(
                bossTransform.position,
                descendTarget,
                dashSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Calcular el hueco seg�n la direcci�n
        // Calcular el hueco (coordenadas siempre consistentes)
        float center = goingRight ? gapPositionRight : gapPositionLeft;
        Vector3 gapLeft = new Vector3(center - (gapWidth / 2f), sweepY, 0);
        Vector3 gapRight = new Vector3(center + (gapWidth / 2f), sweepY, 0);



        // Barrida con hueco
        yield return StartCoroutine(SweepWithGap(descendTarget, gapLeft, gapRight, finalSweepPoint));

        yield return new WaitForSeconds(recoveryTime);

        // Volver a posici�n original
        bossTransform.position = originalPosition;

        Debug.Log("Ataque de barrida completado.");
    }

    private IEnumerator SweepAcross(Vector3 start, Vector3 end)
    {
        bossTransform.position = start;

        while (Vector3.Distance(bossTransform.position, end) > 0.1f)
        {
            bossTransform.position = Vector3.MoveTowards(
                bossTransform.position,
                end,
                dashSpeed * Time.deltaTime
            );

            yield return null;
        }

        bossTransform.position = end;
    }

    private IEnumerator SweepWithGap(Vector3 start, Vector3 gapLeft, Vector3 gapRight, Vector3 end)
    {
        bossTransform.position = start;

        // Punto donde se detiene (borde del hueco)
        Vector3 stopPoint = goingRight ? gapLeft : gapRight;

        float t = 0f;
        float distanceToGap = Vector3.Distance(bossTransform.position, stopPoint);

        while (Vector3.Distance(bossTransform.position, stopPoint) > 0.1f)
        {
            // Aumentar la velocidad gradualmente desde sweepStartSpeed a sweepMaxSpeed
            float progress = Mathf.Clamp01(t / sweepAccelerationTime);
            float currentSpeed = Mathf.Lerp(sweepStartSpeed, sweepMaxSpeed, progress);

            bossTransform.position = Vector3.MoveTowards(
                bossTransform.position,
                stopPoint,
                currentSpeed * Time.deltaTime
            );

            t += Time.deltaTime;
            yield return null;
        }

        // Quedarse quieto (simula que deja el hueco)
        bossTransform.position = stopPoint;
    }




    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        float zDistance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        // Hueco derecha (amarillo)
        Gizmos.color = Color.yellow;
        Vector3 gapStartRight = new Vector3(gapPositionRight - (gapWidth / 2f), sweepY, 0);
        Gizmos.DrawWireCube(gapStartRight + new Vector3(gapWidth / 2f, 0, 0), new Vector3(gapWidth, 0.5f, 0.5f));

        // Hueco izquierda (cyan)
        Gizmos.color = Color.cyan;
        Vector3 gapStartLeft = new Vector3(gapPositionLeft - (gapWidth / 2f), sweepY, 0);
        Gizmos.DrawWireCube(gapStartLeft + new Vector3(gapWidth / 2f, 0, 0), new Vector3(gapWidth, 0.5f, 0.5f));
    }
}