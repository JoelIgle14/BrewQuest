using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Control de combate")]
    public int maxAttacksBeforeRest = 4;
    public int currentAttackCount = 0;
    private bool isResting = false;
    private bool canTakeDamage = false;

    public float restDuration = 7f;

    [Header("Ataques del boss")]
    public List<MonoBehaviour> attackScriptsRaw; // Scripts que implementan IBossAttack
    private List<IBossAttack> attacks = new List<IBossAttack>();

    [Header("Testing")]
    public bool manualControl = false; // Si es true, solo ataques manuales con teclado

    void Start()
    {
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

        int attackIndex = Random.Range(0, attacks.Count);
        yield return StartCoroutine(attacks[attackIndex].Execute());

        currentAttackCount++;

        if (currentAttackCount >= maxAttacksBeforeRest)
        {
            isResting = true;
        }
    }

    IEnumerator RestPhase()
    {
        Debug.Log("Boss está recargando... ¡es tu momento!");
        canTakeDamage = true;

        yield return new WaitForSeconds(restDuration);

        canTakeDamage = false;
        currentAttackCount = 0;
        isResting = false;
    }

    public void ReceiveDamage(int amount)
    {
        if (!canTakeDamage) return;

        Debug.Log("Boss recibió daño!");
        // Aquí iría la lógica para reducir vida del jefe
    }
}
