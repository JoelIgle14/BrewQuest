using UnityEngine;

public class ActivarAliado : MonoBehaviour
{
    public GameObject aliado;
    public GameObject Dialogo;
    public GameObject jugador;

    private NewBehaviourScript habilidadesJugador;
    private Collider2D zonaActivacion;  // Collider de la zona de activación


    private void Start()
    {
        if (aliado != null)
            aliado.SetActive(false);

        if (Dialogo != null)
            Dialogo.SetActive(false);

        if (jugador != null)
            habilidadesJugador = jugador.GetComponent<NewBehaviourScript>();

        // Obtener el collider de la zona de activación
        zonaActivacion = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el jugador colisiona con la zona de activación
        if (collision.CompareTag("Player"))
        {
            if (aliado != null)
                aliado.SetActive(true);

            if (Dialogo != null)
                Dialogo.SetActive(true);

            if (habilidadesJugador != null)
                DesactivarHabilidades();

            // Desactivar el collider para que no se vuelva a activar
            if (zonaActivacion != null)
                zonaActivacion.enabled = false;  // Desactivar el collider

            // Opcional: Destruir el objeto de la zona de activación para eliminarlo por completo
            // Destroy(gameObject);  // Descomenta esto si quieres destruir la zona de activación

            // Opcional: Desactivar el propio script para evitar más interacciones
            // this.enabled = false;
        }
    }

    private void DesactivarHabilidades()
    {
        habilidadesJugador.canJump = false;
        habilidadesJugador.canAttack = false;
        habilidadesJugador.canMove = false;
        habilidadesJugador.canDash = false;
        habilidadesJugador.canDoubleJump = false;
        habilidadesJugador.canShoot = false;

        // Detener movimiento
        if (jugador != null)
        {
            // Desactivar el script de movimiento
            var movimientoJugador = jugador.GetComponent<PlayerMovement>();
            if (movimientoJugador != null)
            {
                movimientoJugador.enabled = false; // Desactivamos el control de movimiento
            }

            // Detener la velocidad si hay un Rigidbody
            Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;  // Detener el movimiento inmediatamente
            }
        }
    }

    private void ReactivarHabilidades()
    {
        habilidadesJugador.canJump = true;
        habilidadesJugador.canAttack = true;
        habilidadesJugador.canMove = true;

        // Reactiva solo lo que el jugador ya tenía
        habilidadesJugador.canDash = GameManager.Instance.hasDash;
        habilidadesJugador.canDoubleJump = GameManager.Instance.hasDoubleJump;
        habilidadesJugador.canShoot = GameManager.Instance.hasShoot;

        if (habilidadesJugador.canShoot)
            jugador.GetComponent<Disparo>().ActivarPowerUp();

        // Reactivar movimiento
        if (jugador != null)
        {
            var movimientoJugador = jugador.GetComponent<PlayerMovement>();
            if (movimientoJugador != null)
            {
                movimientoJugador.enabled = true; // Reactivar el control de movimiento
            }
        }

        // Desactivar el aliado cuando el diálogo termine
        if (aliado != null)
            aliado.SetActive(false);  // Desactivamos el aliado
    }

    private void Update()
    {
        // Aquí verificamos si el panel de diálogo ha desaparecido
        // Esto asume que el script de diálogo (scriptdialogos) tiene alguna propiedad
        // que nos permita verificar si el diálogo ha terminado.
        if (Dialogo != null && !Dialogo.activeSelf)
        {
            ReactivarHabilidades();
        }
    }
}
