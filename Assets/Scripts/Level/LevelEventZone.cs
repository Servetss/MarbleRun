using Barmetler;
using Barmetler.RoadSystem;
using System.Linq;
using UnityEngine;

public class LevelEventZone : MonoBehaviour
{
    [SerializeField] private Transform _start;

    [SerializeField] private Transform _finish;

    [SerializeField] private Transform _xZone;

    [SerializeField] private Transform _directionJump;

    [Header("Splines")]
    [SerializeField] private Road _startSpline;

    [SerializeField] private Road _boostSpline;

    private StartZone _startZone;

    private Road _trackSpline;

    private void Awake()
    {
        _startZone = _start.GetComponent<StartZone>();
    }

    public Vector3 DirectionJump => (_directionJump.position - BoostSplinePoint[BoostSplinePoint.Length - 1].position).normalized;

    public Vector3 ForwardOfDirectionJump()
    {
        Vector3 direction = _directionJump.position - BoostSplinePoint[BoostSplinePoint.Length - 1].position;

        direction.y = 0;
        
        return direction.normalized;
    }

    public Vector3 XZonePosition => _xZone.position;

    public Bezier.OrientedPoint[] StartSplinePoints { get; private set; }

    public Bezier.OrientedPoint[] RoadOrientedPoint { get; private set; }

    public Bezier.OrientedPoint[] BoostSplinePoint { get; private set; }

    public float RoadDistance { get; private set; }

    public void SetEventsToTrack(Road road)
    {
        _trackSpline = road;

        RoadOrientedPoint = road.GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(road.transform)).ToArray();

        StartSplinePoints = _startSpline.GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(_startSpline.transform)).ToArray();

        SetStartZone();

        SetBoostAndFinish();

        BoostSplinePoint = _boostSpline.GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(_boostSpline.transform)).ToArray();

        RoadDistance = _startSpline.GetLength() + _trackSpline.GetLength() + _boostSpline.GetLength();
    }

    public void SetPlayersOnStartZone(Transform[] enemies, Transform player)
    {
        _startZone.SetMarbelsToPosition(enemies, _startSpline);

        _startZone.SetPlayerPosition(player, _startSpline);
    }

    public Road MarbleRoad(int index) // 0 - Start || 1 - Track || 2- BoostZone
    {
        switch (index)
        {
            case 0:
                return _startSpline;
            case 1:
                return _trackSpline;
            case 2:
                return _boostSpline;
        }

        return null;
    }

    private void SetStartZone()
    {
        _start.position = RoadOrientedPoint[0].position;

        _start.rotation = Quaternion.LookRotation(RoadOrientedPoint[0].normal, RoadOrientedPoint[0].forward);
    }

    private void SetBoostAndFinish()
    {
        _finish.position = RoadOrientedPoint[RoadOrientedPoint.Length - 1].position;

        _finish.rotation = Quaternion.LookRotation(RoadOrientedPoint[RoadOrientedPoint.Length - 1].normal, RoadOrientedPoint[RoadOrientedPoint.Length - 1].forward);
    }
}
