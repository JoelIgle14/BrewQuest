using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class espada1 : MonoBehaviour
{
    public Animator animator;
    
    public void TriggerAtaque()
    {
        animator.SetTrigger("espadazo");
    }

}
