using UnityEngine;
using System.Collections;

public class Cartita : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float attackSpeed = 6f;
    public float detectionDistance = 6f;
    public float attackCooldown = 2f;
    public float hoverHeight = 4f;

    public Transform[] patrolPoints;

    private int currentPatrolIndex = 0;
    private GameObject player;
    private Vector3 initialPosition;
    private float cooldownTimer = 0f;
    public bool isBeingHit = false;

    private Animator animator;
    private Rigidbody2D rb;

    private bool knockbackApplied = false;

    private enum EstadoVolador
    {
        Patrullando,
        Persiguiendo,
        Volviendo,
        Golpeado
    }

    private EstadoVolador estado = EstadoVolador.Patrullando;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = GameObject.Find("Jarry");
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (estado != EstadoVolador.Golpeado)
        {
            cooldownTimer -= Time.deltaTime;
            knockbackApplied = false;
        }

        if (isBeingHit)
        {
            estado = EstadoVolador.Golpeado;
        }

        switch (estado)
        {
            case EstadoVolador.Patrullando:
                Patrullar();
                animator.SetTrigger("casual");
                break;

            case EstadoVolador.Persiguiendo:
                Perseguir();
                animator.SetTrigger("rage");
                break;

            case EstadoVolador.Volviendo:
                VolverAPatrullar();
                animator.SetTrigger("casual");
                break;

            case EstadoVolador.Golpeado:
                animator.SetTrigger("hit");

                if (!knockbackApplied)
                {
                    Vector2 direccion = (transform.position - player.transform.position).normalized;
                    AplicarRetroceso(direccion, 10f); // fuerza ajustable
                    knockbackApplied = true;
                    isBeingHit = false;
                }
                break;
        }
    }

    void Patrullar()
    {
        if (patrolPoints.Length > 0)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }

        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distToPlayer <= detectionDistance && cooldownTimer <= 0f)
        {
            estado = EstadoVolador.Persiguiendo;
        }
    }

    void Perseguir()
    {
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

    public void AplicarRetroceso(Vector2 direccion, float fuerza)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
        StartCoroutine(StunCoroutine());
    }

    IEnumerator StunCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        estado = EstadoVolador.Volviendo;
    }
}
