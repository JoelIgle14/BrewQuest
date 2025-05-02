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
            float speed = 0f;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                speed = 1f;

            anim.SetFloat("Speed", speed);
        }
    }
}
