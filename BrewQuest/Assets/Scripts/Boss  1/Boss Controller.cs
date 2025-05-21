using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxAttacksBeforeRest = 4;
    public int currentAttackCount = 0;
    private bool isResting = false;
    private bool canTakeDamage = false;

    public float restDuration = 7f;

    // Aquí arrastras tus scripts de ataque en el inspector
    [Header("Ataques del boss")]
    public List<MonoBehaviour> attackScriptsRaw; // deben implementar IBossAttack
    private List<IBossAttack> attacks = new List<IBossAttack>();

    // Prefabs de hordas
    [Header("Prefabs de hordas")]
    public List<GameObject> hordaPrefabs;

    void Start()
    {
        // Verificamos que todos los scripts implementen la interfaz
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

        StartCoroutine(BossLoop());
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
        int attackIndex = Random.Range(0, attacks.Count);
        yield return StartCoroutine(attacks[attackIndex].Execute());

        currentAttackCount++;

        if (Random.value < 0.4f)
        {
            SpawnHorde();
        }

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

    void SpawnHorde()
    {
        if (hordaPrefabs.Count == 0) return;

        int hordeIndex = Random.Range(0, hordaPrefabs.Count);
        Instantiate(hordaPrefabs[hordeIndex], transform.position, Quaternion.identity);
    }

    public void ReceiveDamage(int amount)
    {
        if (!canTakeDamage) return;

        Debug.Log("Boss recibió daño!");
        // Aquí iría la lógica para restar vida
    }
}
