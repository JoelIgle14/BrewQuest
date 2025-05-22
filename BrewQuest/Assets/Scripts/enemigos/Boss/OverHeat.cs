using System.Collections;
using UnityEngine;

public class CargaPresionAttack : MonoBehaviour, IBossAttack
{
    public float cargaDuration = 2f;        
    public float explosionRadius = 4f;       
    public int damage = 1;                  
    public float recoveryDuration = 1.5f;    

    public GameObject explosionEffectPrefab; 
    public LayerMask playerLayer;            

    private Transform bossTransform;

    private void Awake()
    {
        bossTransform = transform;
    }

    public IEnumerator Execute()
    {
       
        Debug.Log("El boss comienza a cargar presión...");
       
        yield return new WaitForSeconds(cargaDuration);

        
        Debug.Log("¡EXPLOSIÓN de vapor!");
        Explode();

        
        yield return new WaitForSeconds(recoveryDuration);
    }

    private void Explode()
    {
        
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, bossTransform.position, Quaternion.identity);
        }
        
        Collider2D hit = Physics2D.OverlapCircle(bossTransform.position, explosionRadius, playerLayer);
    }

    private void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
