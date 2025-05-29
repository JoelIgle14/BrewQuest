using UnityEngine;

public class Cartita : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float attackSpeed = 6f;
    public float detectionDistance = 6f;
    public float attackCooldown = 2f;
    public float hoverHeight = 4f;

    public Transform[] patrolPoints; // Puntos entre los que vuela en modo patrulla

    private int currentPatrolIndex = 0;
    private GameObject player;
    private Vector3 initialPosition;
    private bool returningToPatrol = false;
    private float cooldownTimer = 0f;

    private enum EstadoVolador
    {
        Patrullando,
        Persiguiendo,
        Volviendo
    }

    private EstadoVolador estado = EstadoVolador.Patrullando;

    private void Start()
    {
        player = GameObject.Find("Jarry");
        initialPosition = transform.position;
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        switch (estado)
        {
            case EstadoVolador.Patrullando:
                Patrullar();
                break;

            case EstadoVolador.Persiguiendo:
                Perseguir();
                break;

            case EstadoVolador.Volviendo:
                VolverAPatrullar();
                break;
        }
    }

    void Patrullar()
    {
        // Movimiento entre puntos
        if (patrolPoints.Length > 0)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }

        // Detección del jugador
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distToPlayer <= detectionDistance && cooldownTimer <= 0f)
        {
            estado = EstadoVolador.Persiguiendo;
        }
    }

    void Perseguir()
    {
        // Baja a la altura del jugador y se lanza hacia él
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, attackSpeed * Time.deltaTime);

        float distToPlayer = Vector2.Distance(transform.position, targetPos);
        if (distToPlayer < 0.5f)
        {
            cooldownTimer = attackCooldown;
            estado = EstadoVolador.Volviendo;
        }
    }

    void VolverAPatrullar()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].position, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.2f)
        {
            estado = EstadoVolador.Patrullando;
        }
    }
}
