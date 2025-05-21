using UnityEngine;

public class FogZone : MonoBehaviour
{
    public SpriteRenderer fogVisual;
    public Collider2D fogCollider;

    public void ShowFog()
    {
        Debug.Log("Niebla puesta");
        gameObject.SetActive(true);
        fogVisual.enabled = true;
        fogCollider.enabled = false;
    }

    public void ActivateDamage()
    {
        fogCollider.enabled = true;
    }

    public void Deactivate()
    {
        fogCollider.enabled = false;
        fogVisual.enabled = false;
    }
}
