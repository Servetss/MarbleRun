using UnityEngine;

public class AIHorizontalMove : MonoBehaviour
{
    [Range(0.01f, 1)]
    [SerializeField] private float _positionChangeSpeed;

    [SerializeField] private float _delayForChangePosition;

    private EventMachine _playerEventMachine;

    private Transform _mesh;

    private float _border = 6;

    private bool _isMove;

    private float _lerp;

    private float _position;

    private float _fromSide;

    private float _toSide;

    private void Awake()
    {
        _mesh = transform.GetChild(0);

        _playerEventMachine = GetComponent<EventMachine>();
    }

    private void Start()
    {
        _playerEventMachine?.SubscribeOnRoadStartStart(SetDefaultData);

        _playerEventMachine?.SubscribeOnRoadStartStart(DelayMove);

        _playerEventMachine?.SubscribeOnBoostZoneStart(BoostZone);
    }

    private void FixedUpdate()
    {
        if (_isMove)
        {
            _lerp += Time.fixedDeltaTime * _positionChangeSpeed;

            _position = Mathf.Lerp(_fromSide, _toSide, _lerp);

            _mesh.localPosition = new Vector3(_position, _mesh.localPosition.y, _mesh.localPosition.z);

            if (_lerp >= 1)
            {
                _isMove = false;

                _fromSide = _toSide;

                _lerp = 0;

                DelayMove();
            }
        }
    }

    private void DelayMove()
    {
        Invoke("ChangeSpeed", _delayForChangePosition);
    }

    private void ChangeSpeed()
    {
        _toSide = Random.Range(-_border, _border);

        _isMove = true;
    }

    private void BoostZone()
    {
        _isMove = false;

        CancelInvoke("ChangeSpeed");
    }

    private void SetDefaultData()
    {
        _position = _mesh.localPosition.x;

        _fromSide = _position;

        _lerp = 0;
    }
}
