using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class espada1 : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            animator.SetTrigger("espadazo");
        }
    }

}
