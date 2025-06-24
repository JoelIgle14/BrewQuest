using UnityEngine;

public class FogZone : MonoBehaviour
{
    public SpriteRenderer fogVisual;
    public Collider2D fogCollider;
    private Material fogMaterial;

    void Awake()
    {
        if (fogVisual != null)
        {
            fogMaterial = fogVisual.material;
        }
    }

    public void ShowFog()
    {
        Debug.Log("Niebla puesta");
        gameObject.SetActive(true);
        fogVisual.enabled = true;
        fogCollider.enabled = false;

        if (fogMaterial != null)
        {
            fogMaterial.SetFloat("_BlinkSpeed", 2.0f);
            fogMaterial.SetFloat("_BlinkEnabled", 1.0f); // Activar parpadeo
        }
    }

    public void ActivateDamage()
    {
        fogCollider.enabled = true;

        if (fogMaterial != null)
        {
            fogMaterial.SetFloat("_BlinkSpeed", 0.0f);
            fogMaterial.SetFloat("_BlinkEnabled", 0.0f); // Desactivar parpadeo, niebla visible fija
        }
    }


    public void Deactivate()
    {
        fogCollider.enabled = false;
        fogVisual.enabled = false;
        gameObject.SetActive(false);
    }
}
