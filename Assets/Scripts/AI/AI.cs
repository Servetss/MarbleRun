using Firebase.RemoteConfig;
using UnityEngine;

public class AI : MonoBehaviour
{
    public const string AccelerationAI = "ai_acceleration";

    [SerializeField] private AIAcceleration _aiAcceleration;

    private AIAcceleration _accelerator;

    private EventMachine _eventMachine;

    private int _saveLevel;
    
    private void Awake()
    {
        _eventMachine = GetComponent<EventMachine>();

        _accelerator = GetComponent<AIAcceleration>();
    }

    public void StartRoad()
    {
        SetNewMaxSpeed(_saveLevel);

        _eventMachine?.RoadStartMethod();
    }

    public void MoveToNextLevel()
    {
        _eventMachine?.NextLevelMethod();
    }

    public void SetNewMaxSpeed(int levelBoost)
    {
        // default balance 3.2

        _saveLevel = levelBoost;

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        float Acceleration = 3.2f;

        if (remoteConfig != null && RemoteConfig.Instance.IsLoaded)
            Acceleration = (float)remoteConfig.GetValue(AccelerationAI).DoubleValue;
        
        float speedValue = Acceleration;

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
