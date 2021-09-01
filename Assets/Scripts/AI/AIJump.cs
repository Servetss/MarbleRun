using UnityEngine;

public class AIJump : MonoBehaviour
{
    [SerializeField] private Finish _finish;

    private EventMachine _eventMachine;

    private RoadMover _roadMover;

    private float _jumpStrenght = 10;

    private Vector3[] _trajectoryPoints;

    private int _jumpCount;

    private int _maxJumpCount = 1;

    private float _xZoneYWeight;

    // LERP //
    private bool _isJump;

    private bool _isLerpMove;

    private float _lerp;

    private int _pointIndex;

    private void Awake()
    {
        _roadMover = GetComponent<RoadMover>();

        _eventMachine = GetComponent<EventMachine>();
    }

    private void Start()
    {
        _eventMachine?.SubscribeOnRoadEnd(JumpStart);
    }

    private void FixedUpdate()
    {
        if (_isJump)
        {
            if (_isLerpMove)
            {
                _lerp += Time.fixedDeltaTime;

                transform.position = Vector3.Lerp(_trajectoryPoints[_pointIndex], _trajectoryPoints[_pointIndex + 1], _lerp);

                if (1 >= _lerp)
                {
                    _lerp = 0;

                    _pointIndex++;

                    if (_pointIndex > 10 && _xZoneYWeight >= transform.position.y)// _pointIndex >= _trajectoryPoints.Length - 1)
                    {
                        if (_jumpCount <= _maxJumpCount)
                        {
                            _isLerpMove = false;

                            _isJump = false;

                            _jumpCount++;

                            Impulse(_jumpStrenght / 1.5f);
                        }
                        else
                        {
                            _isLerpMove = false;

                            _isJump = false;

                            _jumpCount = 0;

                            _eventMachine.FinishMethod();
                        }
                    }
                }
            }
        }
    }

    private void JumpStart()
    {
        Impulse(_roadMover.Speed / 4);
    }

    private void Impulse(float strenght)
    {
        _jumpStrenght = strenght;

        Debug.DrawLine(transform.position, transform.position + (_finish.LevelEventZone.DirectionJump * strenght), Color.red, 100);

        _trajectoryPoints = ShowTrajectory(transform.position, _finish.LevelEventZone.DirectionJump * strenght);

        _pointIndex = 0;

        _xZoneYWeight = _finish.LevelEventZone.XZonePosition.y;

        _isJump = true;

        _isLerpMove = true;
    }

    private Vector3[] ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;

            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;
        }

        return points;
    }
}
