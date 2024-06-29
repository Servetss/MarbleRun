using UnityEngine;

public class AIAcceleration : MonoBehaviour
{
    [SerializeField] private RoadMover _playerRoadMover;

    [Range(1f, 5)]    
    [SerializeField] private float _lerpSpeed = 2f;

    [SerializeField] private float _delayForChangeAcceleration;

    private EventMachine _playerEventMachine;

    private RoadMover _roadMover;

    private float _speedBoost;

    private bool _isBoostZoneAndPlayerAboutToLose;

    private bool _isSpeedChanging;

    private float _lerp;

    private float _speed = 36;

    [Space]
    [SerializeField] private float _fromSpeed;

    [SerializeField] private float _toSpeed;

    public float MinimalSpeed { get => NormalSpeed - (NormalSpeed * 0.2f); }

    public float MaximalSpeed { get => NormalSpeed + (SpeedBalance.SpeedIncrease * 0.1f); }

    private float NormalSpeed { get => SpeedBalance.StartSpeed + (SpeedBalance.SpeedIncrease * _speedBoost); }

    private void Awake()
    {
        _roadMover = GetComponent<RoadMover>();

        _playerEventMachine = GetComponent<EventMachine>();
    }
    private void Start()
    {
        _playerEventMachine?.SubscribeOnRoadStartStart(ChangeSpeed);

        _playerEventMachine?.SubscribeOnBoostZoneStart(SlowDown);

        //_speed = NormalSpeed;

        //_fromSpeed = _speed;
    }

    private void Update()
    {
        if (_isSpeedChanging)
        {
            if (_roadMover.SplineIndex != 2)
            {
                _lerp += Time.deltaTime * _lerpSpeed;
                
                _speed = Mathf.Lerp(_fromSpeed, _toSpeed, _lerp);

                _roadMover.SetSpeed(_speed);

                if (_lerp >= 1)
                {
                    _isSpeedChanging = false;

                    _lerp = 0;

                    _fromSpeed = _speed;

                    DelaySpeedChange();
                }
            }
            else // WHEN BOOST ZONE //
            {
                if (_speed < MaximalSpeed + (MaximalSpeed * 0.3f))
                {   
                    if (_isBoostZoneAndPlayerAboutToLose)
                    {
                        _roadMover.SubstractSpeed(0.01f);
                    }
                    else
                    {
                        _speed += Time.deltaTime * (Time.deltaTime * 0.5f);

                        _roadMover.SetSpeed(_speed);
                    }
                }
            }
        }
    }

    public void SetBoost(float boost)
    {
        _speedBoost = boost;

        _speed = NormalSpeed;

        _fromSpeed = _speed;

        _toSpeed = _speed;
    }

    public void ChangeFromSpeedToSpeed(int value)
    {
        _fromSpeed += value;

        _toSpeed += value;

        _speed += value;

        _roadMover.SetSpeed(_speed);
    }

    private void DelaySpeedChange()
    {
        Invoke("ChangeSpeed", _delayForChangeAcceleration);
    }

    private void ChangeSpeed()
    {
        _fromSpeed = _speed;

        _toSpeed = Random.Range(MinimalSpeed, MaximalSpeed);

        _isSpeedChanging = true;
    }

    private void SlowDown()
    {
        _isBoostZoneAndPlayerAboutToLose = _roadMover.Distance - _playerRoadMover.Distance > 15;
    }
}
