using System.Collections;
using UnityEngine;

public class BossIntroController : MonoBehaviour
{
    public BossComicTransition transicion;

    void Start()
    {
        StartCoroutine(transicion.TransicionBossCompleta());
    }
}
