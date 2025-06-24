using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectoHit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Material bossMaterial;
    private Coroutine flashCoroutine;

    void Start()
    {
        if (spriteRenderer != null)
            bossMaterial = spriteRenderer.material;
    }

    public void FlashWhite(float duration)
    {
        if (bossMaterial == null) return;

        
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(DoFlash(duration));
    }

    private IEnumerator DoFlash(float duration)
    {
        bossMaterial.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(duration);
        bossMaterial.SetFloat("_FlashAmount", 0f);
        flashCoroutine = null;
    }
}
