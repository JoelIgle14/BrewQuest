using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossanimator : MonoBehaviour
{
    private Animator anim;
    private BossController bc;
    private float wait;

    void Start()
    {
        anim = GetComponent<Animator>();
        bc = GetComponent<BossController>();
    }

    
    void Update()
    {
        anim.SetInteger("bn", bc.currentAttackCount);

        if (bc.currentAttackCount < 4)
        {
            StartCoroutine(Change(3f));
        }
    }

    IEnumerator Change(float wait)
    {
        yield return new WaitForSeconds(wait);
        anim.SetTrigger("charge");
    }



}
