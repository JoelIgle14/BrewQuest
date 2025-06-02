using System.Collections;
using UnityEngine;

public interface IBossAttack
{
    IEnumerator Execute();
    //Vector3? GetDesiredPosition(); // null si no requiere moverse

}