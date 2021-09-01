using UnityEngine;

public enum Demention { X, Y, Z}

public class Spinner : MonoBehaviour
{
    [SerializeField] private EventMachine _playerEventMachine;

    [SerializeField] private float _speed;

    [SerializeField] private Demention _dementionRotate;

    private Vector3 _savedVector;

    private float _spinnengAngle;

    public bool IsPlayed { get; private set; }

    private void Start()
    {
        _savedVector = transform.localEulerAngles;

        if (_playerEventMachine != null)
        {
            _playerEventMachine.SubscribeOnRoadStartStart(EnableSpinning);

            _playerEventMachine.SubscribeOnFinish(DisableSpinning);
        }
        else
        {
            EnableSpinning();
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayed)
        {
            _spinnengAngle += Time.fixedDeltaTime * _speed;

            if (_spinnengAngle >= 360) _spinnengAngle = 0;

            switch (_dementionRotate)
            {
                case Demention.X:
                    transform.localEulerAngles = new Vector3(_spinnengAngle, _savedVector.y, _savedVector.z);
                    break;
                case Demention.Y:
                    transform.localEulerAngles = new Vector3(_savedVector.x, _spinnengAngle, _savedVector.z);
                    break;
                case Demention.Z:
                    transform.localEulerAngles = new Vector3(_savedVector.x, _savedVector.y, _spinnengAngle);
                    break;
            }
        }
    }

    public void EnableSpinning()
    {
        IsPlayed = true;
    }

    public void DisableSpinning()
    {
        IsPlayed = false;
    }
}
