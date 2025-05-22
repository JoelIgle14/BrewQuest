using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttack : MonoBehaviour
{
    public string nombre;
    public AnimationClip animacion;

    public IEnumerator Execute()
    {
        // Aquí va la lógica del ataque, efectos, delays, etc.
        Debug.Log("Ejecutando ataque: " + nombre);
        yield return new WaitForSeconds(animacion.length); // Espera duración de la animación
    }
}

