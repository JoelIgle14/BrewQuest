using UnityEngine;
using UnityEngine.UI;

public class IndicadorTiros : MonoBehaviour
{
    [Header("Sprites de la jarra (0 a 5 tiros)")]
    public Sprite[] spritesCerveza; // Index 0 = vacía, 5 = llena

    [Header("Componente de Imagen UI")]
    public Image imagenUI;

    [Header("Referencia al script Disparo")]
    public Disparo disparoScript;

    void Update()
    {
        if (disparoScript != null && disparoScript.PowerUpActivo)
        {
            int tiros = Mathf.Clamp(disparoScript.tirosDisponibles, 0, 5);
            imagenUI.sprite = spritesCerveza[tiros];
        }
        else
        {
            // Muestra jarra vacía si no tiene el power-up activo
            imagenUI.sprite = spritesCerveza[0];
        }
    }
}
