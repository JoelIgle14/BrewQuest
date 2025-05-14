using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogos : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_Text dialogoTexto;
    public GameObject panelDialogo;

    [Header("Diálogos iniciales")]
    [TextArea(2, 4)]
    public List<string> dialogosIniciales;

    private int dialogoIndex = 0;
    private bool mostrandoDialogo = true;
    private bool escribiendo = false;
    private Coroutine escrituraActual;

    private void Start()
    {
        // Activa panel y muestra primer diálogo
        panelDialogo.SetActive(true);
        MostrarDialogo(dialogosIniciales[dialogoIndex]);
    }

    private void Update()
    {
        if (!mostrandoDialogo) return;

        if (Input.anyKeyDown)
        {
            if (escribiendo)
            {
                TerminarEscrituraInstantanea();
            }
            else
            {
                dialogoIndex++;
                if (dialogoIndex < dialogosIniciales.Count)
                {
                    MostrarDialogo(dialogosIniciales[dialogoIndex]);
                }
                else
                {
                    // Fin del diálogo
                    mostrandoDialogo = false;
                    panelDialogo.SetActive(false);

                }
            }
        }
    }

    private void MostrarDialogo(string texto)
    {
        if (escrituraActual != null)
            StopCoroutine(escrituraActual);

        escrituraActual = StartCoroutine(MaquinaDeEscribir(texto));
    }

    private IEnumerator MaquinaDeEscribir(string texto)
    {
        escribiendo = true;
        dialogoTexto.text = "";
        foreach (char letra in texto)
        {
            dialogoTexto.text += letra;
            yield return new WaitForSeconds(0.02f);
        }
        escribiendo = false;
    }

    private void TerminarEscrituraInstantanea()
    {
        if (escrituraActual != null)
            StopCoroutine(escrituraActual);

        dialogoTexto.text = dialogosIniciales[dialogoIndex];
        escribiendo = false;
    }

    public bool DialogoActivo()
    {
        return mostrandoDialogo;
    }

    public void IniciarDialogoConRetraso()
    {
        StartCoroutine(IniciarDialogoConRetrasoCoroutine());
    }

    private IEnumerator IniciarDialogoConRetrasoCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        panelDialogo.SetActive(true);
        MostrarDialogo(dialogosIniciales[dialogoIndex]);
    }
}
