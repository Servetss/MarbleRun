using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Accelerator : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private ParticleSystem _speedParticle;

    private EventMachine _playerEventMachine;

    private RoadMover _roadMover;

    [Header("Debug")]
    [SerializeField] private float _speedBoost;

    private bool _isRelised;

    private bool _isPlayeMode;

    public float MinimalSpeed { get => MaximalSpeed - (MaximalSpeed * 0.2f); }

    public float MaximalSpeed { get => SpeedBalance.StartSpeed + (SpeedBalance.SpeedIncrease * _speedBoost); } // NORMAL SPEED

    public float BoostMaximalSpeed { get => MaximalSpeed + (MaximalSpeed * 1f); }

    private void Awake()
    {
        _roadMover = GetComponent<RoadMover>();

        _playerEventMachine = GetComponent<EventMachine>();
    }

    private void Start()
    {
        _speed = MinimalSpeed;

        _playerEventMachine.SubscribeOnRoadStartStart(AccelerateModeEnable);

        _playerEventMachine.SubscribeOnBoostZoneStart(AccelerateModeDisable);

        _playerEventMachine.SubscribeOnFinish(ResetData);
    }

    private void Update()
    {
        if (_isPlayeMode) // Мод звичайної їзди по треку
        {
            _isRelised = true;

            // VERIOSN 0.92 //
            //if (Input.GetMouseButtonDown(0))
            //{
            //    _isRelised = true;
            //}

            //if (Input.GetMouseButtonUp(0))
            //{
            //    _isRelised = false;
            //}

            if ((_isRelised == false && _speed == MinimalSpeed) == false && (_isRelised && _speed == MaximalSpeed) == false)
            {
                //_speed = _isRelised ? _speed + (Time.deltaTime * _acceleration) : _speed - (Time.deltaTime * _acceleration);

                _speed = Mathf.Clamp(_speed, 0, MaximalSpeed);

                _roadMover.SetSpeed(_speed);
            }

            ChangeSpeed(Time.deltaTime * 1);
        }
        else if (_playerEventMachine.PlayerState == PlayerState.BoostZone)
        {
            if (_roadMover.Speed > MaximalSpeed)
            {
                // Поки швидкість більше мінімальної, то поступово зменшуй швидкість
                // Швидкість буде набиратись на клік
                ChangeSpeed(Time.deltaTime * -0.15f);
                
                float newSpeed = _roadMover.Speed + (Time.deltaTime * -0.15f);

                _roadMover.SetSpeed(newSpeed);
            }
        }
    }

    private void FixedUpdate()
    {
        SpeedParticleActivness();
    }

    public void ChangeSpeed(float value)
    {
        _speed += value;
    }

    public void SubtractSpeed(float percentValue)
    {
        percentValue = Mathf.Clamp01(percentValue);

        _speed = _speed - (_speed * percentValue);
    }

    public void SetAcceleration(float acceleration)
    {
        // OLD //
    }

    public void SetSpeedBoost(float boost)
    {
        _speedBoost = boost;
    }

    private void SpeedParticleActivness()
    {
        if (_isPlayeMode)
        {
            if (_speedParticle.isPlaying == false && _speed >= (MaximalSpeed - 1))
                _speedParticle.Play();
            else if(_speedParticle.isPlaying && _speed < (MaximalSpeed - 1))
                _speedParticle.Stop();
        }
        else if (_isPlayeMode == false && _speedParticle.isPlaying)
        {
            _speedParticle.Stop();
        }
    }

    private void AccelerateModeEnable()
    {
        _isPlayeMode = true;
    }

    private void AccelerateModeDisable()
    {
        _isPlayeMode = false;
    }

    private void ResetData()
    {
        _speed = MinimalSpeed;

        _roadMover.SetSpeed(_speed);
    }
}
