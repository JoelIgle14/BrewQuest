using UnityEngine;

public class WeaponFollower : MonoBehaviour
{
    public Transform weaponTransform;       // Asigna la espada aquí
    public Vector2 idleOffset = Vector2.zero;
    public Vector2 runOffset = Vector2.zero;

    private Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        if (weaponTransform == null)
        {
            Debug.LogWarning("WeaponFollower: No se ha asignado el transform del arma.");
        }


    }

    void Update()
    {
        if (weaponTransform == null || animator == null)
            return;

        string currentAnim = GetCurrentAnimationName();

        if (currentAnim.ToLower().Contains("idle"))
        {
            weaponTransform.localPosition = idleOffset;
        }
        else if (currentAnim.ToLower().Contains("run"))
        {
            weaponTransform.localPosition = runOffset;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("espadazo");
        }
    }

    string GetCurrentAnimationName()
    {
        if (animator.GetCurrentAnimatorClipInfoCount(0) > 0)
        {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
                return clipInfo[0].clip.name;
        }

        return "";
    }
}

