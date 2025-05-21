using System.Collections;
using UnityEngine;

public class Martillo : MonoBehaviour, IBossAttack
{
    public Transform target;              
    public float levitationHeight = 5f;   
    public float levitationDuration = 1f; 
    public float pauseBeforeFall = 1f;    
    public float fallSpeed = 15f;         
    public float recoveryTime = 1f;       

    private Vector3 originalPosition;

    private Transform bossTransform;

    private void Awake()
    {
        bossTransform = transform;
    }

    public IEnumerator Execute()
    {
        originalPosition = bossTransform.position;

        Debug.Log("MArtillooooo");


        Vector3 targetPosAbove = new Vector3(target.position.x, target.position.y + levitationHeight, target.position.z);
        yield return MoveToPosition(targetPosAbove, levitationDuration);

        
        yield return new WaitForSeconds(pauseBeforeFall);

     
        Vector3 fallTargetPos = new Vector3(target.position.x, target.position.y, target.position.z);
        yield return FallToPosition(fallTargetPos, fallSpeed);

       
        yield return new WaitForSeconds(recoveryTime);

     
        yield return MoveToPosition(originalPosition, levitationDuration);
    }

    private IEnumerator MoveToPosition(Vector3 targetPos, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = bossTransform.position;

        while (elapsed < duration)
        {
            bossTransform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossTransform.position = targetPos;
    }

    private IEnumerator FallToPosition(Vector3 targetPos, float speed)
    {
        while (bossTransform.position.y > targetPos.y)
        {
            bossTransform.position += Vector3.down * speed * Time.deltaTime;
            yield return null;
        }

        bossTransform.position = targetPos;
    }
}
