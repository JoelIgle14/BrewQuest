using UnityEngine;
using System.Collections;

public class JarraVida : MonoBehaviour
{
    public Sprite jarraLlena;
    public Sprite jarraMedia;
    public Sprite jarraCasiVacia;
    public Sprite jarraVacia;

    private SpriteRenderer spriteRenderer;
    public int vidas = 3;

    private bool gameOverIniciado = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ActualizarSprite();
    }

    void Update()
    {
        // Prueba: presiona espacio para perder vida
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerderVida();
        }
    }

    public void PerderVida()
    {
        if (vidas > 0)
        {
            vidas--;
            ActualizarSprite();

            if (vidas == 0 && !gameOverIniciado)
            {
                gameOverIniciado = true;
                StartCoroutine(EsperarYGameOver());
            }
        }
    }

    void ActualizarSprite()
    {
        switch (vidas)
        {
            case 3:
                spriteRenderer.sprite = jarraLlena;
                break;
            case 2:
                spriteRenderer.sprite = jarraMedia;
                break;
            case 1:
                spriteRenderer.sprite = jarraCasiVacia;
                break;
            case 0:
                spriteRenderer.sprite = jarraVacia;
                break;
        }
    }

    IEnumerator EsperarYGameOver()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("¡Game Over!");
    }
}
