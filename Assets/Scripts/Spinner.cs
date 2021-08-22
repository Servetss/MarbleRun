using UnityEngine;

public enum Demention { X, Y, Z}

public class Spinner : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Demention _dementionRotate;

    private float _spinnengAngle;
    
    private void FixedUpdate()
    {
        _spinnengAngle += Time.fixedDeltaTime * _speed;

        if (_spinnengAngle >= 360) _spinnengAngle = 0;

        switch (_dementionRotate)
        {
            case Demention.X:
                transform.localEulerAngles = new Vector3(_spinnengAngle, 0, 0);
                break;
            case Demention.Y:
                transform.localEulerAngles = new Vector3(0, _spinnengAngle, 0);
                break;
            case Demention.Z:
                transform.localEulerAngles = new Vector3(0, 0, _spinnengAngle);
                break;
        }
        
    }
}
