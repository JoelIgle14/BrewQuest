using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour, IBossAttack
{
    public List<FogZone> fogZones;
    public float warningDuration = 2f; 
    public float activeDuration = 4f;  

    public IEnumerator Execute()
    {
        if (fogZones.Count < 5)
        {
            Debug.LogWarning("FogAttack necesita al menos 5 zonas.");
            yield break;
        }

        
        List<FogZone> selectedZones = new List<FogZone>(fogZones);
        Shuffle(selectedZones);
        List<FogZone> activeZones = selectedZones.GetRange(0, 5); 

        
        foreach (var zone in activeZones)
        {
            zone.ShowFog();
        }

        yield return new WaitForSeconds(warningDuration);

      
        foreach (var zone in activeZones)
        {
            zone.ActivateDamage();
        }

        yield return new WaitForSeconds(activeDuration);

        foreach (var zone in activeZones)
        {
            zone.Deactivate();
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
