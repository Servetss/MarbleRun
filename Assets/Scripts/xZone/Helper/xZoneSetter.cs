using UnityEngine;

public class xZoneSetter : MonoBehaviour
{
    // Header("Platform Transform Settings") //
    private const int _platformLength = 20;

    // Header("BOOST") //
    private const float _startBoost = 1;

    private const float _increaseBoost = 0.1f;

    private Vector3 _startLocalPostion;

    private void Start()
    {
        InitZones();
    }
    
    [ContextMenu("Init Zones")]
    public void InitZones()
    {
        _startLocalPostion = transform.GetChild(0).localPosition;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform zone = transform.GetChild(i);

            SetZonePosition(zone, i);

            SetZoneBoost(zone, i);
        }
    }

    private void SetZonePosition(Transform zone, int index)
    {
        Vector3 forward = Vector3.up;
        
        zone.localPosition = _startLocalPostion + (_platformLength * forward) * index;
    }

    private void SetZoneBoost(Transform zone, int index)
    {
        zone.GetComponent<BoostPlatform>().SetBoost(index, _startBoost, _increaseBoost);
    }
}
