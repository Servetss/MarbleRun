using UnityEngine;

public class AI : MonoBehaviour
{
    private EventMachine _eventMachine;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _eventMachine = GetComponent<EventMachine>();

        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StartRoad()
    {
        _eventMachine?.RoadStartMethod();
    }

    public void MoveToNextLevel()
    {
        _rigidbody.isKinematic = true;

        _rigidbody.useGravity = false;

        _eventMachine?.NextLevelMethod();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "BoostZone")
        {
            _eventMachine?.BoostZoneStartMethod();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BoostZone")
        {
            _eventMachine?.BoostZoneFinishMethod();
        }
    }
}
