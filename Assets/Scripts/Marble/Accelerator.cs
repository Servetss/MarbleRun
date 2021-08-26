using UnityEngine;

public class Accelerator : MonoBehaviour
{
    private const int MinimalSpeed = 30;

    private const int MaximalSpeed = 60;

    private EventMachine _playerEventMachine;

    private RoadMover _roadMover;

    private float _acceleration = 8;

    private float _speed;

    private bool _isRelised;

    private bool _isPlayeMode;

    private void Awake()
    {
        _roadMover = GetComponent<RoadMover>();

        _playerEventMachine = GetComponent<Player>().PlayerEventMachine;
    }

    private void Start()
    {
        _speed = MinimalSpeed;

        _playerEventMachine.SubscribeOnRoadStartStart(AccelerateModeEnable);

        _playerEventMachine.SubscribeOnNormalLevelEnd(AccelerateModeDisable);

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
