using UnityEngine;

public class Accelerator : MonoBehaviour
{
    [SerializeField] private float _speedBoost;

    [SerializeField] private float _acceleration = 8;

    private EventMachine _playerEventMachine;

    private RoadMover _roadMover;

    private float _speed;

    private bool _isRelised;

    private bool _isPlayeMode;

    private void Awake()
    {
        _roadMover = GetComponent<RoadMover>();

        _playerEventMachine = GetComponent<EventMachine>();
    }

    private float MinimalSpeed {get => _speedBoost + 30;}

    private float MaximalSpeed { get => _speedBoost + 60; }

    private void Start()
    {
        _speed = MinimalSpeed;

        _playerEventMachine.SubscribeOnRoadStartStart(AccelerateModeEnable);

        _playerEventMachine.SubscribeOnBoostZoneStart(AccelerateModeDisable);

        _playerEventMachine.SubscribeOnFinish(ResetData);
    }

    private void Update()
    {
        if (_isPlayeMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isRelised = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isRelised = false;
            }

            if ((_isRelised == false && _speed == MinimalSpeed) == false && (_isRelised && _speed == MaximalSpeed) == false)
            {
                _speed = _isRelised ? _speed + (Time.deltaTime * _acceleration) : _speed - (Time.deltaTime * _acceleration);

                _speed = Mathf.Clamp(_speed, MinimalSpeed, MaximalSpeed);

                _roadMover.SetSpeed(_speed);
            }
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
