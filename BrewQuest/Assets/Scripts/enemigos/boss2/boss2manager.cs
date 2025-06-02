using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2manager : MonoBehaviour
{
    [Header("Control de combate")]
    //public int maxAttacksBeforeRest = 4;
    //public int currentAttackCount = 0;
    //private bool isResting = false;

    public float restDuration = 7f;

    [Header("Ataques del boss")]
    public List<MonoBehaviour> attackScriptsRaw; // Scripts que implementan IBossAttack
    private List<Iboss2atac> attacks = new List<Iboss2atac>();

    [Header("Testing")]
    public bool manualControl = false; // Si es true, solo ataques manuales con teclado

    private Rigidbody2D rb2d;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isDead;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("No hay Rigidbody2D en el boss.");
        }
        else
        {
            rb2d.bodyType = RigidbodyType2D.Static; // Inicialmente está estático
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Convertimos y validamos los scripts referenciados
        foreach (var script in attackScriptsRaw)
        {
            if (script is Iboss2atac attack)
            {
                attacks.Add(attack);
            }
            else
            {
                Debug.LogWarning($"{script.name} no implementa Iboss2atac.");
            }
        }

        if (attacks.Count == 0)
        {
            Debug.LogError("No hay ataques válidos referenciados en BossController.");
        }

        if (!manualControl)
        {
            StartCoroutine(BossLoop());
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
            yield return StartCoroutine(AttackPhase());
            yield return new WaitForSeconds(1.5f); // Pausa entre ataques
        }
    }

    IEnumerator AttackPhase()
    {
        if (attacks.Count == 0)
        {
            Debug.LogWarning("No hay ataques disponibles.");
            yield break;
        }

        int attackIndex = Random.Range(0, attacks.Count);
        yield return StartCoroutine(attacks[attackIndex].Execute());
    }

    //IEnumerator RestPhase()
    //{
    //    Debug.Log("Boss está recargando... ¡es tu momento!");


    //    rb2d.bodyType = RigidbodyType2D.Dynamic;
    //    rb2d.gravityScale = 1f;

    //    yield return new WaitForSeconds(restDuration);

    //    rb2d.bodyType = RigidbodyType2D.Static;
    //    rb2d.gravityScale = 0f;

    //    transform.position = originalPosition;
    //    transform.rotation = originalRotation;

    //    currentAttackCount = 0;
    //    isResting = false;
    //}
}
