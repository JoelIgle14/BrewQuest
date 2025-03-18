using UnityEngine;

public class AnimationLimiter : MonoBehaviour
{
    private Animator animator;
    public string animationName; // Nombre de la animación
    public int maxRepeats = 1; // Número de repeticiones, se ajusta en Unity
    public bool moreAnimsAfter = true; // Activar si hay más animaciones después, para que no las pare y solo pare la actual
    public MonoBehaviour movementScript; // Referencia al script de movimiento

    private int repeatCount = 0;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Desactiva el script de movimiento mientras se ejecuta la animación de Spawn
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
    }

    // Esta función debe ser llamada al final de la animación desde un evento
    public void OnAnimationEnd()
    {
        repeatCount++;
        if (repeatCount < maxRepeats)
        {
            animator.Play(animationName, 0, 0); // Reinicia la animación
        }
        else
        {
            // Reactiva el movimiento después de la animación
            if (movementScript != null)
            {
                movementScript.enabled = true;
            }

            if (!moreAnimsAfter)
            {
                animator.enabled = false; // Detiene el Animator tras la última repetición
            }
        }
    }
}
