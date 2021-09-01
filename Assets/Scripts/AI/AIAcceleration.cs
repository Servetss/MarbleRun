using UnityEngine;

public class AIAcceleration : MonoBehaviour
{
    [Range(0.5f, 5)]
    [SerializeField] private float _lerpSpeed = 2;

    [SerializeField] private float _delayForChangeAcceleration;

    [SerializeField] private float _minimumSpeed;

    [SerializeField] private float _maximumSpeed;

    private EventMachine _playerEventMachine;

    private RoadMover _roadMover;

    private bool _isSpeedChanging;

    private float _lerp;

    private float _speed = 30;

    private float _fromSpeed;

    private float _toSpeed;

    private void Awake()
    {
        _roadMover = GetComponent<RoadMover>();

        _playerEventMachine = GetComponent<EventMachine>();
    }
    private void Start()
    {
        _fromSpeed = _speed;

        _playerEventMachine?.SubscribeOnRoadStartStart(DelaySpeedChange);
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
            else
            {
                if (_speed < 90)
                {
                    _speed += Time.deltaTime * 10;

                    _roadMover.SetSpeed(_speed);
                }
            }
        }
    }

    private void DelaySpeedChange()
    {
        Invoke("ChangeSpeed", _delayForChangeAcceleration);
    }

    private void ChangeSpeed()
    {
        _toSpeed = Random.Range(_minimumSpeed, _maximumSpeed);

        _isSpeedChanging = true;
    }
}
