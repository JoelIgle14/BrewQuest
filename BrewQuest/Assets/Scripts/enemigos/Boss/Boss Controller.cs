using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IBossIntroReceiver
{
    [Header("Control de combate")]
    public int maxAttacksBeforeRest = 4;
    public int currentAttackCount = 0;
    private bool isResting = false;
    public bool canTakeDamage = false;

    public float restDuration = 7f;

    [Header("Ataques del boss")]
    public List<MonoBehaviour> attackScriptsRaw; // Scripts que implementan IBossAttack
    private List<IBossAttack> attacks = new List<IBossAttack>();

    [SerializeField] private GameObject vulnerableMessage;

    [Header("Intro del jefe")]
    public bool introTerminada = false; // Se pone a true desde la transición

    [Header("Testing")]
    public bool manualControl = false; // Si es true, solo ataques manuales con teclado

    private Rigidbody2D rb2d;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isDead;

    public float timeBetweenAttacks = 2f;  // Tiempo de calma entre ataque
    private int lastAttackIndex = -1;  // Guarda el �ltimo �ndice usado

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("No hay Rigidbody2D en el boss.");
        }
        else
        {
            rb2d.bodyType = RigidbodyType2D.Static; // Inicialmente est� est�tico
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Convertimos y validamos los scripts referenciados
        foreach (var script in attackScriptsRaw)
        {
            if (script is IBossAttack attack)
            {
                attacks.Add(attack);
            }
            else
            {
                Debug.LogWarning($"{script.name} no implementa IBossAttack.");
            }
        }

        if (attacks.Count == 0)
        {
            Debug.LogError("No hay ataques v�lidos referenciados en BossController.");
        }

        if (!manualControl)
        {
            StartCoroutine(EsperarIntroYComenzar());
        }

    }

    void Update()
    {
        if (!manualControl) return;

        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                if (attacks.Count >= i)
                {
                    Debug.Log($"Ejecutando ataque manual #{i}");
                    StartCoroutine(attacks[i - 1].Execute());
                }
                else
                {
                    Debug.LogWarning($"No hay ataque asignado para la tecla {i}");
                }
            }
        }
    }

    IEnumerator BossLoop()
    {
        while (true)
        {
            if (isResting)
            {
                yield return StartCoroutine(RestPhase());
            }
            else
            {
                yield return StartCoroutine(AttackPhase());
            }
        }
    }

    IEnumerator AttackPhase()
    {
        if (attacks.Count == 0)
        {
            Debug.LogWarning("No hay ataques disponibles.");
            yield break;
        }

        int attackIndex;
        do
        {
            attackIndex = Random.Range(0, attacks.Count);
        }
        while (attackIndex == lastAttackIndex && attacks.Count > 1);

        lastAttackIndex = attackIndex;
        var attack = attacks[attackIndex];

        //// Mover suavemente si el ataque requiere una posición específica
        //Vector3? targetPos = attack.GetDesiredPosition();
        //if (targetPos.HasValue)
        //{
        //    yield return StartCoroutine(MoveToPosition(targetPos.Value));
        //}

        yield return StartCoroutine(attack.Execute());

        currentAttackCount++;
        yield return new WaitForSeconds(timeBetweenAttacks);

        if (currentAttackCount >= maxAttacksBeforeRest)
        {
            isResting = true;
        }
    }
    IEnumerator RestPhase()
    {
        Debug.Log("Boss está recargando... ¡es tu momento!");
        canTakeDamage = true;

        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 1f;

        if (vulnerableMessage != null)
            vulnerableMessage.SetActive(true);

        yield return new WaitForSeconds(restDuration);

        if (vulnerableMessage != null)
            vulnerableMessage.SetActive(false);

        rb2d.bodyType = RigidbodyType2D.Static;
        rb2d.gravityScale = 0f;

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        canTakeDamage = false;
        currentAttackCount = 0;
        isResting = false;
    }



    IEnumerator EsperarIntroYComenzar()
    {
        while (!introTerminada)
        {
            yield return null;
        }

        StartCoroutine(BossLoop());
    }

    public void NotificarIntroTerminada()
    {
        introTerminada = true;
    }


}
