using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jarryanim : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        }
    }
}
