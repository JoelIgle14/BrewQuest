using System.Collections;
using UnityEngine;

public class Boss2vida : MonoBehaviour
{
    public float health;
    public bool golpeado;
    public int puntos = 1000;
    private Boss2manager bc;
    public GameObject player;
    private Animator anim;
    private AudioController controller;
    private bool muriendo = false;

    void Awake()
    {
        bc = GetComponent<Boss2manager>();
        anim = GetComponent<Animator>();
        controller = FindObjectOfType<AudioController>();
    }

    public void TakeDamage(float amount, GameObject Player, bool esAtaqueCuerpoACuerpo)
    {
        if (!golpeado && bc != null && !muriendo)
        {
            controller.SeleccionAudio(6, 0.2f);
            anim.SetTrigger("hit");
            golpeado = true;
            health -= amount;

            if (health <= 0)
            {
                muriendo = true;

                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(puntos);
                }

                StartCoroutine(MuerteBoss());
                return;
            }

            if (esAtaqueCuerpoACuerpo)
            {
                Disparo disparo = Player.GetComponent<Disparo>();
                if (disparo != null)
                {
                    disparo.RecargarTiro();
                }
            }

            StartCoroutine(ResetGolpeado());
        }
    }

    private IEnumerator MuerteBoss()
    {
        // Ir al centro de la pantalla
        Vector3 centroPantalla = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        centroPantalla.z = transform.position.z;
        transform.position = centroPantalla;

        // Vibración/temblor
        float shakeDuration = 1f;
        float shakeAmount = 0.2f;
        Vector3 originalPos = transform.position;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-shakeAmount, shakeAmount);
            float offsetY = Random.Range(-shakeAmount, shakeAmount);
            transform.position = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;

        // Activar animación de explosión
        if (anim != null)
        {
            anim.SetTrigger("die");
        }

        yield return new WaitForSeconds(1f); // ajusta si tu animación tarda más

        Destroy(gameObject);
        ActivateDoor();
    }

    private void ActivateDoor()
    {
        GameObject puerta = GameObject.Find("Puerta2");
        if (puerta != null)
        {
            puerta.GetComponent<SpriteRenderer>().enabled = true;
            puerta.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            Debug.LogWarning("No puerta jeje");
        }
    }

    private IEnumerator ResetGolpeado()
    {
        yield return new WaitForSeconds(1f);
        golpeado = false;
    }
}
