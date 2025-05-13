using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public GameObject cartel;     // Cartel del paso
        public string accion;         // Acción que se espera (Mover, Saltar, etc.)
    }

    [Header("Referencias")]
    public TMP_Text dialogoTexto;
    public GameObject panelDialogo;
    public MonoBehaviour scriptMovimientoJugador; // El script que quieres desactivar (ej: PlayerMovement)

    [Header("Diálogos iniciales")]
    [TextArea(2, 4)]
    public List<string> dialogosIniciales;

    [Header("Pasos del tutorial")]
    public List<TutorialStep> pasos;

    private int pasoActual = 0;
    private int dialogoIndex = 0;
    private bool mostrandoDialogo = true;
    private bool escribiendo = false;
    private Coroutine escrituraActual;

    private void Start()
    {
        // Desactiva carteles del tutorial
        foreach (var paso in pasos)
        {
            paso.cartel.SetActive(false);
        }

        // Bloquea movimiento del jugador
        if (scriptMovimientoJugador != null)
            scriptMovimientoJugador.enabled = false;

        // Activa panel y muestra primer diálogo
        panelDialogo.SetActive(true);
        MostrarDialogo(dialogosIniciales[dialogoIndex]);
    }

    private void Update()
    {
        // Modo diálogo
        if (mostrandoDialogo)
        {
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

                        if (scriptMovimientoJugador != null)
                            scriptMovimientoJugador.enabled = true; // Reactiva movimiento

                        if (pasos.Count > 0)
                        {
                            pasos[pasoActual].cartel.SetActive(true);
                        }
                    }
                }
            }

            return; // ¡IMPORTANTE! No continúa al modo tutorial si hay diálogo
        }

        // Modo tutorial (acciones del jugador)
        if (pasoActual >= pasos.Count) return;

        if (AccionCompletada(pasos[pasoActual].accion))
        {
            pasos[pasoActual].cartel.SetActive(false);
            pasoActual++;

            if (pasoActual < pasos.Count)
            {
                pasos[pasoActual].cartel.SetActive(true);
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

    private bool AccionCompletada(string accion)
    {
        switch (accion)
        {
            case "Movimiento":
                return Input.GetAxisRaw("Horizontal") != 0;
            case "Salto":
                return Input.GetKeyDown(KeyCode.Space);
            case "Dash":
                return Input.GetKeyDown(KeyCode.E);
            case "Ataque":
                return Input.GetKeyDown(KeyCode.Q);
            default:
                return false;
        }
    }

    // Este método permite a otros scripts preguntar si hay diálogo activo
    public bool DialogoActivo()
    {
        return mostrandoDialogo;
    }
}
