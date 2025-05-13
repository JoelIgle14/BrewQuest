using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogoAliado : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_Text dialogoTexto;
    public GameObject panelDialogo;
    public MonoBehaviour scriptMovimientoJugador; // Desactiva movimiento
    public List<string> dialogos;

    private int dialogoIndex = 0;
    private bool mostrandoDialogo = false;
    private bool escribiendo = false;
    private Coroutine escrituraActual;

    private void Start()
    {
        panelDialogo.SetActive(false);
    }

    public void IniciarDialogo()
    {
        if (scriptMovimientoJugador != null)
            scriptMovimientoJugador.enabled = false;

        dialogoIndex = 0;
        mostrandoDialogo = true;
        panelDialogo.SetActive(true);
        MostrarDialogo(dialogos[dialogoIndex]);
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
                if (dialogoIndex < dialogos.Count)
                {
                    MostrarDialogo(dialogos[dialogoIndex]);
                }
                else
                {
                    TerminarDialogo();
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

        dialogoTexto.text = dialogos[dialogoIndex];
        escribiendo = false;
    }

    private void TerminarDialogo()
    {
        mostrandoDialogo = false;
        panelDialogo.SetActive(false);
        if (scriptMovimientoJugador != null)
            scriptMovimientoJugador.enabled = true;
    }
}
