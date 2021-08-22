using Barmetler;
using System;
using UnityEngine;

public class RoadMover : MonoBehaviour
{
    private Bezier.OrientedPoint[] _roadOrientedPoint;

    [Range(0.1f, 150)]
    [SerializeField] private float _speed = 2;

    private int _pointIndex;

    private float _lerp;

    private bool _isMove;

    private Vector3 _startPosition;

    private Vector3 _directiobPosition;

    private Quaternion _startRotation;

    private Quaternion _directionRotation;

    private Action Finish;

    public void SetRoad(Bezier.OrientedPoint[] orientedPoints)
    {
        _roadOrientedPoint = orientedPoints;

        StartPosition();
    }

    private void Update()
    {
        if (_isMove)
        {
            _lerp += Time.deltaTime * _speed;

            Vector3 position = Vector3.Lerp(_startPosition, _directiobPosition, _lerp) + _roadOrientedPoint[_pointIndex].normal / 2;

            Quaternion rotation = Quaternion.LerpUnclamped(_startRotation, _directionRotation, _lerp);

            transform.position = position;

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

    public void SetSpeed(int value)
    {
        _speed = value;
    }

    private void StartPosition()
    {
        _startPosition = _roadOrientedPoint[_pointIndex].position;

        if (_pointIndex + 1 >= _roadOrientedPoint.Length)
        {
            Finish();

            return;
        }

        _directiobPosition = _roadOrientedPoint[_pointIndex + 1].position;

        _startRotation = Quaternion.LookRotation(_roadOrientedPoint[_pointIndex].forward, _roadOrientedPoint[_pointIndex].normal);

        _directionRotation = Quaternion.LookRotation(_roadOrientedPoint[_pointIndex + 1].forward, _roadOrientedPoint[_pointIndex + 1].normal);

        _isMove = true;
    }

    private void LastSplineIndex()
    {
        _pointIndex = 0;

        Finish?.Invoke();
    }


    #region Action
    public void SubscribeOnFinish(Action method)
    {
        Finish += method;
    }
    #endregion
}
