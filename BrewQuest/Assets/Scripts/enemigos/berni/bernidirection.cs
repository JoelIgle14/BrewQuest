using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bernidirection : MonoBehaviour
{
        private GameObject target;

        //[HideInInspector]
        public int lookDirection = 1; // 1 = derecha, -1 = izquierda

        void Start()
        {
            target = GameObject.Find("jarry");
        }

        void Update()
        {
            Vector3 scale = transform.localScale;

            if (transform.position.x > target.transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x) * -1f;
                lookDirection = -1;
            }
            else
            {
                scale.x = Mathf.Abs(scale.x);
                lookDirection = 1;
            }

            transform.localScale = scale;
        }
}
