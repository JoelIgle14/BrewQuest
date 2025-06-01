using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossComicTransition : MonoBehaviour
{
    public Camera camara;
    public Transform objetivoZoom;
    public float zoomDuracion = 1f;
    public float zoomTamano = 3f;
    public Image panelNegro;
    public TextMeshProUGUI textoComic;
    public CameraShake cameraShake;

    private float zoomInicial;
    private Vector3 posicionInicialCamara;

    void Start()
    {
        zoomInicial = camara.orthographicSize;
        posicionInicialCamara = camara.transform.position;

        // Ocultar elementos al inicio
        textoComic.alpha = 0;
        textoComic.transform.localScale = Vector3.zero;
        panelNegro.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator TransicionBossCompleta()
    {
        yield return StartCoroutine(TransicionBoss());
    }

    private IEnumerator TransicionBoss()
    {
        // Guardar datos originales
        float t = 0f;
        Vector3 inicioPos = camara.transform.position;
        Vector3 destino = new Vector3(objetivoZoom.position.x, objetivoZoom.position.y, inicioPos.z);
        float tamOriginal = camara.orthographicSize;

        // Hacer zoom in
        while (t < zoomDuracion)
        {
            t += Time.deltaTime;
            float progress = t / zoomDuracion;

            camara.transform.position = Vector3.Lerp(inicioPos, destino, progress);
            camara.orthographicSize = Mathf.Lerp(tamOriginal, zoomTamano, progress);

            yield return null;
        }

        // Sacudida
        yield return StartCoroutine(cameraShake.Shake(0.4f, 0.25f));

        // Panel negro
        for (float a = 0; a < 1f; a += Time.deltaTime * 2)
        {
            panelNegro.color = new Color(0, 0, 0, a * 0.6f);
            yield return null;
        }

        // Texto cómic
        textoComic.text = "¡PREPÁRATE!";
        textoComic.alpha = 1;
        float scale = 0f;
        while (scale < 1f)
        {
            scale += Time.deltaTime * 2f;
            textoComic.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        // ➕ ZOOM BACK (restaurar posición y tamaño)
        float tBack = 0f;
        while (tBack < zoomDuracion)
        {
            tBack += Time.deltaTime;
            float progress = tBack / zoomDuracion;

            camara.transform.position = Vector3.Lerp(destino, inicioPos, progress);
            camara.orthographicSize = Mathf.Lerp(zoomTamano, tamOriginal, progress);

            yield return null;
        }

        // ⇩ ANIMACIÓN DE DESVANECIMIENTO ⇩
        float fadeOutDuration = 1f;
        float elapsed = 0f;

        float textoAlphaInicial = textoComic.alpha;
        Color panelColorInicial = panelNegro.color;
        Vector3 escalaInicial = textoComic.transform.localScale;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float fadeProgress = elapsed / fadeOutDuration;

            textoComic.alpha = Mathf.Lerp(textoAlphaInicial, 0f, fadeProgress);
            textoComic.transform.localScale = Vector3.Lerp(escalaInicial, Vector3.zero, fadeProgress);
            panelNegro.color = Color.Lerp(panelColorInicial, new Color(0, 0, 0, 0), fadeProgress);


            yield return null;
        }

        // Asegurar valores al final
        textoComic.alpha = 0;
        textoComic.transform.localScale = Vector3.zero;
        panelNegro.color = new Color(0, 0, 0, 0);

        // Activar IA del boss
        BossController boss = FindObjectOfType<BossController>();
        if (boss != null)
        {
            boss.introTerminada = true;
        }


    }

}
