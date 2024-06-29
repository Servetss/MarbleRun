using UnityEngine;

public class AI : MonoBehaviour
{
    public const string AccelerationAI = "ai_acceleration";

    public const float StartSpeed = 3.2f;

    [SerializeField] private AIAcceleration _aiAcceleration;

    private AIAcceleration _accelerator;

    private EventMachine _eventMachine;

    private int _saveLevel;

    private float _saveSpeed;

    private void Awake()
    {
        _eventMachine = GetComponent<EventMachine>();

        _accelerator = GetComponent<AIAcceleration>();
    }

    public void StartRoad()
    {
        SetBoost(_saveLevel, _saveSpeed);

        _eventMachine?.RoadStartMethod();
    }

    public void MoveToNextLevel()
    {
        _eventMachine?.NextLevelMethod();
    }

    public void SetBoost(int levelBoost, float speed)
    {
        _saveLevel = levelBoost;

        _saveSpeed = speed;
        
        _aiAcceleration.SetBoost(speed);
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
