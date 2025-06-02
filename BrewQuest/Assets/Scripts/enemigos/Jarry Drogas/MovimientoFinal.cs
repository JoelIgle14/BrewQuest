using UnityEngine;
using System.Collections;

public class MovimientoFinal : MonoBehaviour
{
    public bool final = false;
    public bool textoFinal = false;

    public float distanciaDetrasJarry = 1.0f;
    public float velocidadMovimiento = 10f;

    private bool enProceso = false;
    private GameObject jarry;
    private Animator animator;
    private GameObject audioManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioManager = GameObject.Find("AudioManager");

        if (audioManager == null)
        {
            Debug.LogError("No se encontró el objeto 'AudioManager'");
        }
    }

    private void Update()
    {
        if (final && !enProceso)
        {
            StartCoroutine(ProcesoFinal());
        }
    }

    private IEnumerator ProcesoFinal()
    {
        enProceso = true;

        jarry = GameObject.Find("Jarry");
        if (jarry == null)
        {
            Debug.LogError("No se encontró el objeto 'Jarry'");
            yield break;
        }

        // Llamar método "dash" en AudioManager si lo tiene
        if (audioManager != null)
        {
            audioManager.SendMessage("dash", SendMessageOptions.DontRequireReceiver);
        }

        Vector3 posicionObjetivo = jarry.transform.position - jarry.transform.forward * distanciaDetrasJarry;
        float tiempo = 0f;
        Vector3 posicionInicial = transform.position;

        while (tiempo < 0.2f)
        {
            tiempo += Time.deltaTime * velocidadMovimiento;
            transform.position = Vector3.Lerp(posicionInicial, posicionObjetivo, tiempo / 0.2f);
            yield return null;
        }

        transform.position = posicionObjetivo;

        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("start");

        yield return new WaitForSeconds(1f);
        animator.SetTrigger("stort");

        // Llamar método "Muerte" en AudioManager si lo tiene
        if (audioManager != null)
        {
            audioManager.SendMessage("Muerte", SendMessageOptions.DontRequireReceiver);
        }

        textoFinal = true;
    }
}
