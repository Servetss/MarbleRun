using Barmetler.RoadSystem;
using System;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] private Transform _playerStartPosition;

    [SerializeField] private Transform[] _marbleStartPositions;

    public void SetMarbelsToPosition(Transform[] marbles, Road startZone)
    {
        if (marbles.Length > _marbleStartPositions.Length)
            throw new ArgumentException();

        for (int i = 0; i < marbles.Length; i++)
        {
            SetPositionOnStartZone(marbles[i], _marbleStartPositions[i], startZone);
        }
    }

    public void SetPlayerPosition(Transform player, Road startZone)
    {
        SetPositionOnStartZone(player, _playerStartPosition, startZone);
    }

    private void SetPositionOnStartZone(Transform marble, Transform onStartPosition, Road startZone)
    {
        RoadMover roadMover = marble.GetComponent<RoadMover>();

        SlideMover slideMover = marble.GetComponent<SlideMover>();

        Vector3 vectorOnTheSpline = startZone.OrientedPoints[startZone.GetIndexOnPlineByTransform(onStartPosition)].position;

        int side = GetSide(onStartPosition.position, vectorOnTheSpline) > 0 ? 1 : -1;

        float distance = Vector3.Distance(onStartPosition.position, vectorOnTheSpline) * side;

        marble.rotation = Quaternion.LookRotation(startZone.OrientedPoints[0].forward, startZone.OrientedPoints[0].normal);

        marble.position = vectorOnTheSpline + startZone.OrientedPoints[0].normal / 2;

        marble.GetChild(0).localPosition = new Vector3(distance, marble.GetChild(0).localPosition.y, marble.GetChild(0).localPosition.z);

        roadMover.SetSplineIndex(startZone.GetIndexOnPlineByTransform(onStartPosition));

        roadMover.SetDistance(startZone.GetIndexOnPlineByTransform(onStartPosition) * Vector3.Distance(startZone.OrientedPoints[0].position, startZone.OrientedPoints[1].position));

        slideMover?.SetActualPosition(distance);
    }

    private double GetSide(Vector3 from, Vector3 to)
    {
        double x = from.x - to.x;
        double y = from.y - to.y;
        double z = from.z - to.z;

        return Math.Sqrt(x + y + z);
    }
}
