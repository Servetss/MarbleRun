using UnityEngine;

public class AI : MonoBehaviour
{
    private EventMachine _eventMachine;

    private void Awake()
    {
        _eventMachine = GetComponent<EventMachine>();
    }

    public void StartRoad()
    {
        _eventMachine?.RoadStartMethod();
    }

    public void StartBoostZone()
    {
        _eventMachine?.BoostZoneStartMethod();
    }

    public void RoadEnd()
    {
        _eventMachine?.BoostZoneFinishMethod();
    }

    public void MoveToNextLevel()
    {
        _eventMachine?.NextLevelMethod();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "BoostZone")
        {
            Debug.Log("AI: " + gameObject.name);

            StartBoostZone();
        }
    }
}
