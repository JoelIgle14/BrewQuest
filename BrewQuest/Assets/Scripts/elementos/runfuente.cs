using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runfuente : MonoBehaviour
{
    private Animator animator;
    public bool canSound = false;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("activa");
            canSound = true;
        }
    }
}
