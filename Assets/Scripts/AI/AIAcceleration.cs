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
    }
    private void Start()
    {
        _fromSpeed = _speed;

        _playerEventMachine?.SubscribeOnRoadStartStart(DelaySpeedChange);
    }

    private void FixedUpdate()
    {
        if (_isSpeedChanging)
        {
            _lerp += Time.fixedDeltaTime * _lerpSpeed;

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
