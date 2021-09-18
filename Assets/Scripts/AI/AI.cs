using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private AIAcceleration _aiAcceleration;

    private AIAcceleration _accelerator;

    private EventMachine _eventMachine;

    private void Awake()
    {
        _eventMachine = GetComponent<EventMachine>();

        _accelerator = GetComponent<AIAcceleration>();
    }

    public void StartRoad()
    {
        _eventMachine?.RoadStartMethod();
    }

    public void MoveToNextLevel()
    {
        _eventMachine?.NextLevelMethod();
    }

    public void SetNewMaxSpeed(int levelBoost)
    {
        float speedValue = 3.2f;

        _aiAcceleration.IncreaseSpeed(levelBoost * speedValue);
    }

    public void OnMarbleSphereTriggerEnter(Collider other)
    {
        if (other.name == "BoostZone")
        {
            _eventMachine?.BoostZoneStartMethod();
        }
        else if (other.GetComponent<Obstacle>())
        {
            other.GetComponent<Obstacle>().DestroyObstacle();

            SoundManager.Instance.OnObstacleCrash(transform.position);

            _accelerator.ChangeFromSpeedToSpeed(-15);
        }
    }

    public void OnMarbleSphereTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BoostZone")
        {
            _eventMachine?.BoostZoneFinishMethod();
        }
    }
}
