using UnityEngine;

public class CreditosScroll : MonoBehaviour
{
    public RectTransform textoCreditos;  // El objeto hijo que se mueve
    public float velocidad = 30f;
    public float alturaLimiteY = 1000f;  // Altura en Y que activa la acción
    private bool finalizado = false;

    void Update()
    {
        if (!finalizado)
        {
            // Mover hacia arriba el textoCreditos (en su RectTransform)
            textoCreditos.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

            // Verificar la posición Y del textoCreditos (local, anclada)
            if (textoCreditos.anchoredPosition.y >= alturaLimiteY)
            {
                finalizado = true;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CargarEscena(0);
                }
                else
                {
                    Debug.LogWarning("No se encontró instancia de GameManager.");
                }
            }
        }
    }
}
