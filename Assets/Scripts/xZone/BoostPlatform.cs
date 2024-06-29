using UnityEngine;

public class BoostPlatform : MonoBehaviour
{
    [SerializeField] private Material[] _materialColors;

    [Space]
    [SerializeField] private xZone[] xZones;

    public void SetBoost(int platformIndex, float startBoost, float increaseBoost)
    {
        float maxValueInPlatform = increaseBoost * xZones.Length;

        float thisPlatformStartBoost = maxValueInPlatform * platformIndex;

        startBoost += thisPlatformStartBoost;

        for (int i = 0; i < xZones.Length; i++)
        {
            xZones[i].SetBoost(startBoost + (increaseBoost * i));
            
            xZones[i].SetMaterial(_materialColors[i]);
        }
    }
}
