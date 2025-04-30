using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public GameObject cartel; // El cartel que se va a mostrar
        public string accion; // Nombre de la acción: "Entrar", "Mover", "Saltar", etc.
    }

    public List<TutorialStep> pasos;
    private int pasoActual = 0;

    private void Start()
    {
        // Desactiva todos los carteles primero
        foreach (var paso in pasos)
        {
            paso.cartel.SetActive(false);
        }

        // Muestra el primer cartel
        if (pasos.Count > 0)
        {
            pasos[pasoActual].cartel.SetActive(true);
        }
    }

    private void Update()
    {
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
}
