using Barmetler;
using Barmetler.RoadSystem;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoadMover : MonoBehaviour
{
    private Bezier.OrientedPoint[] _roadOrientedPoint;

    [SerializeField] private LevelPreparer _levelPreparer;

    [Range(0.1f, 150)]
    [SerializeField] private float _speed = 2;

    [SerializeField] private bool _isPlayer;

    [SerializeField] private float _distance;

    private bool _isCameraNeedBeRotated;

    private int _maximumSplines = 3;

    private int _pointIndex;

    [Range(0, 1)]
    [SerializeField] private float _lerp;

    private Vector3 _previousPosition;

    private Vector3 _startPosition;

    private Vector3 _directiobPosition;

    private Quaternion _startRotation;

    private Quaternion _directionRotation;

    [SerializeField] private Text _speedText;

    private void Awake()
    {
        PlayerEventMachine = GetComponent<EventMachine>();

        PositionOnTheTrackView = GetComponent<PositionOnTheTrackView>();
    }

    public float Speed { get => _speed; }

    public float Distance { get => _distance; }

    public bool IsMove { get; private set; }

    public int SplineIndex { get; private set; }

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
        _distance = distance;
    }

    private void StartMoveBySpline()
    {
        _isCameraNeedBeRotated = SplineIndex < 2;

        Road road = _levelPreparer.LevelEventZone.MarbleRoad(SplineIndex);

        SetRoad(road.GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(road.transform)).ToArray());

        IsMove = true;
    }

    private void SetRoad(Bezier.OrientedPoint[] orientedPoints)
    {
        _roadOrientedPoint = orientedPoints;

        StartPosition();
    }

    private void FixedUpdate()
    {
        //if (_speedText != null)
        //{
        //    _speedText.text = _speed.ToString();
        //}

        if (IsMove)
        {
            if (SplineIndex == 2 && _speed > 60)
            {
                _speed -= Time.fixedDeltaTime * 0.5f;// * 5;
            }

            _lerp += Time.fixedDeltaTime * (_speed * 0.1f);

            Vector3 position = Vector3.Lerp(_startPosition, _directiobPosition, _lerp) + _roadOrientedPoint[_pointIndex].normal / 2;

            Quaternion rotation = Quaternion.LerpUnclamped(_startRotation, _directionRotation, _lerp);

            _distance += Vector3.Distance(transform.position, position);

            transform.position = position;

            if (_isCameraNeedBeRotated)
                transform.rotation = rotation;

            if (_lerp >= 1)
            {
                IsMove = false;

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

        IsMove = true;
    }

    private void LastSplineIndex()
    {
        SplineIndex++;

        _pointIndex = 0;

        IsMove = false;


        if (SplineIndex >= _maximumSplines)
        {
            PlayerEventMachine?.RoadEndMethod();

            _distance = 0;

            SplineIndex = 0;
        }
        else
        {
            StartMoveBySpline();
        }
    }

    private void DisableMover()
    {
        if (IsMove)
        {
            IsMove = false;

            _distance = 0;

            SplineIndex = 0;
        }
    }
}
