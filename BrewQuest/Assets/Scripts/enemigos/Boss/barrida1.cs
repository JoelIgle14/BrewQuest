using System.Collections;
using UnityEngine;

public class barrida1 : MonoBehaviour, IBossAttack
{
    [Header("Puntos de barrida")]
    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    [Header("Parámetros de la barrida")]
    public float chargeTime = 1f;
    public float dashSpeed = 10f;
    public float recoveryTime = 1f;

    [Header("Altura fija para barrida (auto-posición de puntos)")]
    public float sweepY = -2.5f;

    [Header("Parámetros del hueco")]
    public float gapWidth = 2f;

    [Header("Velocidad progresiva de barrida")]
    public float sweepStartSpeed = 1.5f;
    public float sweepMaxSpeed = 12f;
    public float sweepAccelerationTime = 0.5f;

    [Header("Posiciones del hueco")]
    public float gapPositionRight = 0f;
    public float gapPositionLeft = 0f;

    [Header("Justicia visual")]
    public bool showTelegraph = true;
    public float telegraphDuration = 0.5f;

    private Transform bossTransform;
    private Vector3 originalPosition;
    private bool goingRight;
    private SpriteRenderer sr;

    private void Awake()
    {
        bossTransform = transform.root;
        sr = bossTransform.GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator Execute()
    {
        originalPosition = bossTransform.position;

        goingRight = Random.Range(0, 2) == 0;

        Vector3 scale = bossTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * (goingRight ? 1 : -1);
        bossTransform.localScale = scale;

        float zDistance = Mathf.Abs(Camera.main.transform.position.z - bossTransform.position.z);
        Vector3 leftWorld = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, zDistance));
        Vector3 rightWorld = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, zDistance));

        float spawnX = goingRight ? leftWorld.x - 2f : rightWorld.x + 2f;
        float startX = goingRight ? leftWorld.x + 1f : rightWorld.x - 1f;
        float endX = goingRight ? rightWorld.x - 1f : leftWorld.x + 1f;

        Vector3 flyInStart = new Vector3(spawnX, originalPosition.y, 0);
        Vector3 flyInTarget = new Vector3(startX, originalPosition.y, 0);
        Vector3 descendTarget = new Vector3(startX, sweepY, 0);
        Vector3 finalSweepPoint = new Vector3(endX, sweepY, 0);

        // Movimiento suave al inicio para evitar salto brusco
        while (Vector3.Distance(bossTransform.position, flyInStart) > 0.1f)
        {
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, flyInStart, dashSpeed * Time.deltaTime);
            yield return null;
        }

        // Volar hasta el borde de pantalla
        while (Vector3.Distance(bossTransform.position, flyInTarget) > 0.1f)
        {
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, flyInTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(chargeTime);

        // Bajar verticalmente
        while (bossTransform.position.y > sweepY)
        {
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, descendTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }

        // Mostrar telegrafiado visual si está activado
        if (showTelegraph)
            yield return StartCoroutine(TelegraphFlash(telegraphDuration));

        float center = goingRight ? gapPositionRight : gapPositionLeft;
        Vector3 gapLeft = new Vector3(center - (gapWidth / 2f), sweepY, 0);
        Vector3 gapRight = new Vector3(center + (gapWidth / 2f), sweepY, 0);

        yield return StartCoroutine(SweepWithGap(descendTarget, gapLeft, gapRight, finalSweepPoint));

        yield return new WaitForSeconds(recoveryTime);

        yield return StartCoroutine(MoveSmoothlyTo(originalPosition, dashSpeed));

        Debug.Log("Ataque de barrida completado.");
    }

    private IEnumerator TelegraphFlash(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }
    }

    private IEnumerator SweepWithGap(Vector3 start, Vector3 gapLeft, Vector3 gapRight, Vector3 end)
    {
        bossTransform.position = start;
        Vector3 stopPoint = goingRight ? gapLeft : gapRight;

        float t = 0f;
        while (Vector3.Distance(bossTransform.position, stopPoint) > 0.1f)
        {
            float progress = Mathf.Clamp01(t / sweepAccelerationTime);
            float currentSpeed = Mathf.Lerp(sweepStartSpeed, sweepMaxSpeed, progress);

            bossTransform.position = Vector3.MoveTowards(bossTransform.position, stopPoint, currentSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        bossTransform.position = stopPoint;
    }

    private IEnumerator MoveSmoothlyTo(Vector3 target, float speed)
    {
        while (Vector3.Distance(bossTransform.position, target) > 0.1f)
        {
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        bossTransform.position = target;
    }

    public Vector3? GetDesiredPosition()
    {
        return null;
    }

    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        float zDistance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Gizmos.color = Color.yellow;
        Vector3 gapStartRight = new Vector3(gapPositionRight - (gapWidth / 2f), sweepY, 0);
        Gizmos.DrawWireCube(gapStartRight + new Vector3(gapWidth / 2f, 0, 0), new Vector3(gapWidth, 0.5f, 0.5f));

        Gizmos.color = Color.cyan;
        Vector3 gapStartLeft = new Vector3(gapPositionLeft - (gapWidth / 2f), sweepY, 0);
        Gizmos.DrawWireCube(gapStartLeft + new Vector3(gapWidth / 2f, 0, 0), new Vector3(gapWidth, 0.5f, 0.5f));
    }
}