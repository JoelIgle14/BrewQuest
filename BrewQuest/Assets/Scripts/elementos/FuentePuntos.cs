using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private AudioController controller;

    

    public bool yaActivado = false;
    public int puntosPorCheckpoint = 20;

    private void Start()
    {

        controller = FindObjectOfType<AudioController>(); 
        controller.SeleccionAudio(0, 1f);
    }
}


