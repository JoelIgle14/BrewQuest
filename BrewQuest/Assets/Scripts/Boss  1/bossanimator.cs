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
        switch (bc.currentAttackCount)
        {
            case 0:
                anim.SetTrigger("b1");
                break;
            case 1:
                anim.SetTrigger("b2");
                break;
            case 2:
                anim.SetTrigger("b3");
                break;
            case 3:
                anim.SetTrigger("b4");
                break;
            case 4:
                anim.SetTrigger("b5");
                StartCoroutine(Change(3));
                break;
        }
    }

    IEnumerator Change(float wait)
    {
        yield return new WaitForSeconds(wait);
        anim.SetTrigger("charge");
        yield return new WaitForSeconds(wait);
        anim.SetTrigger("return");

    }



}
