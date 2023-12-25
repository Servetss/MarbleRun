using UnityEngine;

public class Jump : MonoBehaviour
{
    [Range(1, 20)]
    public float JumpStrenght;

    [SerializeField] private Finish _finish;

    [SerializeField] private Transform _shadowEffect;

    private Vector3[] _trajectoryPoints;

    //private Vector3 _lerpPosition;
    
    private int _jumpCount;

    private int _maxJumpCount = 10;

    private float _xZoneYWeight;

    // LERP //
    private bool _isJump;

    private bool _isLerpMove;

    private float _lerp;

    private int _pointIndex;

    public Vector3[] TrajectoryPoints { get => _trajectoryPoints; }

    private void FixedUpdate()
    {
        if (_isJump)
        {
            if (_isLerpMove)
            {
                _lerp += Time.fixedDeltaTime;
                
                //_lerpPosition = Vector3.Lerp(_trajectoryPoints[_pointIndex], _trajectoryPoints[_pointIndex + 1], _lerp);
                
                transform.position = Vector3.Lerp(_trajectoryPoints[_pointIndex], _trajectoryPoints[_pointIndex + 1], _lerp);

                if (1 >= _lerp)
                {
                    _lerp = 0;

                    _pointIndex++;
                    
                    float sinLerp = Mathf.Sin(((float)_pointIndex / ((float)_trajectoryPoints.Length * 0.5f)) * Mathf.PI);
                    
                    _shadowEffect.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, sinLerp);
                    
                    if (_pointIndex > 30 && _xZoneYWeight >= transform.position.y)// _pointIndex >= _trajectoryPoints.Length - 1)
                    {
                        if (_jumpCount <= _maxJumpCount)
                        {
                            _isLerpMove = false;

                            _isJump = false;

                            _jumpCount++;

                            _shadowEffect.localScale = Vector3.one;

                            Impulse(JumpStrenght / 1.5f);
                        }
                        else
                        {
                            _isLerpMove = false;

                            _isJump = false;

                            _jumpCount = 0;

                            _finish.MarbleStop(GetComponent<Player>());
                        }
                        SoundManager.Instance.OnXZoneJump();
                    }
                }
            }
        }
    }

    public void Impulse(float strenght)
    {
        JumpStrenght = strenght;

        Vector3 vectorJump = transform.position;

        Vector3 directionJump = _finish.LevelEventZone.DirectionJump;

        _xZoneYWeight = _finish.LevelEventZone.XZonePosition.y;

        if (strenght < 5)
        {
            directionJump = _finish.LevelEventZone.ForwardOfDirectionJump();
        }

        if (vectorJump.y < _xZoneYWeight)
        {
            vectorJump = new Vector3(vectorJump.x, _xZoneYWeight, vectorJump.z);
        }
        
        _trajectoryPoints = ShowTrajectory(vectorJump, directionJump * strenght, _xZoneYWeight);

        _pointIndex = 0;

        _isJump = true;

        _isLerpMove = true;

        RaycastHit hit;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 2);
        
        xZone selectedXZone = null;

        bool isTrigger = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<xZone>())
            {
                isTrigger = true;

                selectedXZone = colliders[i].GetComponent<xZone>();

                break;
            }
        }
        
        if (isTrigger)
        {
            selectedXZone.OnCustomTrigger(strenght);
        }
    }

    private Vector3[] ShowTrajectory(Vector3 origin, Vector3 speed, float minPos)
    {
        Vector3[] points = new Vector3[500];

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.01f;

            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;

            if (points[i].y < minPos)
            {
                points[i] = new Vector3(points[i].x, minPos, points[i].z);
            }
        }

        return points;
    }
}
