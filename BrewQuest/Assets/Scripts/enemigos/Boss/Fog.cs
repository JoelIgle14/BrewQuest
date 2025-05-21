using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour, IBossAttack
{
    public List<FogZone> fogZones; // Las 3 zonas posibles
    public float warningDuration = 2f; // Tiempo antes de que haga daño
    public float activeDuration = 4f;  // Tiempo que permanece activa con daño

    public IEnumerator Execute()
    {
        if (fogZones.Count < 3)
        {
            Debug.LogWarning("FogAttack necesita al menos 3 zonas.");
            yield break;
        }

        // 1. Elegir 2 zonas aleatorias
        List<FogZone> selectedZones = new List<FogZone>(fogZones);
        Shuffle(selectedZones);
        FogZone zone1 = selectedZones[0];
        FogZone zone2 = selectedZones[1];

        // 2. Mostrar niebla sin daño
        zone1.ShowFog();
        zone2.ShowFog();
        yield return new WaitForSeconds(warningDuration);

        // 3. Activar daño
        zone1.ActivateDamage();
        zone2.ActivateDamage();
        yield return new WaitForSeconds(activeDuration);

        // 4. Desactivar
        zone1.Deactivate();
        zone2.Deactivate();
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
