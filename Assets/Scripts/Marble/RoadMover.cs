using Barmetler;
using Barmetler.RoadSystem;
using System.Linq;
using UnityEngine;

public class RoadMover : MonoBehaviour
{
    private Bezier.OrientedPoint[] _roadOrientedPoint;

    [SerializeField] private LevelPreparer _levelPreparer;

    [Range(0.1f, 150)]
    [SerializeField] private float _speed = 2;

    [SerializeField] private bool _isPlayer;

    private bool _isCameraNeedBeRotated;

    private int _splineIndex;

    private int _maximumSplines = 3;

    private int _pointIndex;

    private float _lerp;

    private bool _isMove;

    private Vector3 _startPosition;

    private Vector3 _directiobPosition;

    private Quaternion _startRotation;

    private Quaternion _directionRotation;

    private void Awake()
    {
        PlayerEventMachine = GetComponent<EventMachine>();

        PositionOnTheTrackView = GetComponent<PositionOnTheTrackView>();
    }

    public float Speed { get => _speed; }

    public float Distance { get; private set; }

    public PositionOnTheTrackView PositionOnTheTrackView { get; private set; }

    public EventMachine PlayerEventMachine { get; private set; }

    private void Start()
    {
        PlayerEventMachine?.SubscribeOnRoadStartStart(StartMoveBySpline);

        PlayerEventMachine?.SubscribeOnMoveToNextLevel(DisableMover);
    }

    public void SetSplineIndex(int index)
    {
        _pointIndex = index;
    }

    public void SetDistance(float distance)
    {
        Distance = distance;
    }

    private void StartMoveBySpline()
    {
        _isCameraNeedBeRotated = _splineIndex < 2;

        Road road = _levelPreparer.LevelEventZone.MarbleRoad(_splineIndex);

        SetRoad(road.GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(road.transform)).ToArray());
    }

    private void SetRoad(Bezier.OrientedPoint[] orientedPoints)
    {
        _roadOrientedPoint = orientedPoints;

        StartPosition();
    }

    private void Update()
    {
        if (_isMove)
        {
            _lerp += Time.smoothDeltaTime * _speed;

            Distance += Time.smoothDeltaTime * _speed;

            Vector3 position = Vector3.Lerp(_startPosition, _directiobPosition, _lerp) + _roadOrientedPoint[_pointIndex].normal / 2;

            Quaternion rotation = Quaternion.LerpUnclamped(_startRotation, _directionRotation, _lerp);

            transform.position = position;

            if(_isCameraNeedBeRotated)
                transform.rotation = rotation;

            if (_lerp >= 1)
            {
                _isMove = false;

                _lerp = 0;

                _pointIndex++;

                StartPosition();
            }
        }
    }

    public void Accelerate(float value)
    {
        _speed += value;
    }

    public void SetSpeed(float value)
    {
        _speed = value;
    }

    private void StartPosition()
    {
        _startPosition = _roadOrientedPoint[_pointIndex].position;

        if (_pointIndex + 1 >= _roadOrientedPoint.Length)
        {
            LastSplineIndex();

            return;
        }

        _directiobPosition = _roadOrientedPoint[_pointIndex + 1].position;

        _startRotation = Quaternion.LookRotation(_roadOrientedPoint[_pointIndex].forward, _roadOrientedPoint[_pointIndex].normal);

        _directionRotation = Quaternion.LookRotation(_roadOrientedPoint[_pointIndex + 1].forward, _roadOrientedPoint[_pointIndex + 1].normal);

        _isMove = true;
    }

    private void LastSplineIndex()
    {
        _splineIndex++;

        _pointIndex = 0;

        _isMove = false;


        if (_splineIndex >= _maximumSplines)
        {
            PlayerEventMachine?.RoadEndMethod();

            Distance = 0;

            _splineIndex = 0;
        }
        else
        {
            StartMoveBySpline();
        }
    }

    private void DisableMover()
    {
        if (_isMove)
        {
            _isMove = false;

            Distance = 0;

            _splineIndex = 0;
        }
    }
}
