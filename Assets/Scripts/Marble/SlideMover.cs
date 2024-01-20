using UnityEngine;
using UnityEngine.UI;

public class SlideMover : MonoBehaviour
{
    [SerializeField] private float _border;

    [SerializeField] private float _speed;

    [SerializeField] private Camera _camera;

    [Header("Shadow")]
    [SerializeField] private Transform _shadow;

    [SerializeField] private Transform _shadow2;

    private EventMachine _playerEventMachine;

    private Transform _mesh;

    private Transform _sphereInMesh;

    private Vector3 _clickedPosition;

    private Vector3 _moveMousePosition;

    private float _mouseDiff;

    private float _actualPosition;

    private float _moveRotationY;

    private float _moveRotationX;

    private float _cameraRotateZ;

    private bool _isLerp;

    private float _lerp;

    private float _cameraLerp;
    
    private float _Xposition;

    private float _Yposition;

    private float _cameraZPostion;

    private bool _isCanMove;

    private void Awake()
    {
        _playerEventMachine = GetComponent<EventMachine>();
    }

    private void Start()
    {
        //_camera = Camera.main;//.transform;

        _mesh = transform.GetChild(0);

        _sphereInMesh = _mesh.GetChild(0);

        _playerEventMachine = GetComponent<EventMachine>();

        _playerEventMachine.SubscribeOnRoadStartStart(EnableMove);

        _playerEventMachine.SubscribeOnBoostZoneStart(DisableMove);
    }

    private void Update()
    {
        if (_isCanMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isLerp = false;

                _lerp = 0;

                _cameraLerp = 0;

                _clickedPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                _moveMousePosition = Input.mousePosition;

                _mouseDiff = _moveMousePosition.x - _clickedPosition.x;

                _actualPosition += _mouseDiff * _speed * Time.deltaTime;

                _actualPosition = Mathf.Clamp(_actualPosition, -_border, _border);

                _moveRotationY = Mathf.Clamp(_mouseDiff / 4, -18, 18);

                _moveRotationX = Mathf.Clamp(_mouseDiff / 4, -10, 10);

                _cameraRotateZ = Mathf.Clamp(_mouseDiff / 40, -6, 6);

                _mesh.localPosition = new Vector3(_actualPosition, _mesh.localPosition.y, _mesh.localPosition.z);

                _shadow.localPosition = new Vector3(_mesh.localPosition.x, _shadow.localPosition.y, _shadow.localPosition.z);

                _shadow2.localPosition = new Vector3(_mesh.localPosition.x, _shadow2.localPosition.y, _shadow2.localPosition.z);

                _mesh.localEulerAngles = new Vector3(0, _moveRotationY, -_moveRotationX);

                _camera.transform.localEulerAngles = new Vector3(_camera.transform.localEulerAngles.x, _camera.transform.transform.localEulerAngles.y, -_cameraRotateZ);

                _camera.transform.localPosition = new Vector3(_actualPosition / 1.5f, _camera.transform.localPosition.y, _camera.transform.transform.localPosition.z);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _Xposition = _mesh.localEulerAngles.x;

                _Yposition = _mesh.localEulerAngles.y > 180 ? _mesh.localEulerAngles.y - 360 : _mesh.localEulerAngles.y;

                _cameraZPostion = _camera.transform.localEulerAngles.z > 180 ? _camera.transform.localEulerAngles.z - 360 : _camera.transform.localEulerAngles.z;

                _isLerp = true;
            }
        }

        if (_isLerp)
        {
            _lerp += Time.deltaTime * 0.2f;

            _cameraLerp += Time.deltaTime * 0.4f;

            float x = Mathf.Lerp(_Xposition, 0, _lerp);
            
            float y = Mathf.Lerp(_Yposition, 0, _lerp);

            float z = Mathf.Lerp(_cameraZPostion, 0, _cameraLerp);
            
            _mesh.localEulerAngles = new Vector3(0, y, x);

            _camera.transform.localEulerAngles = new Vector3(_camera.transform.localEulerAngles.x, _camera.transform.localEulerAngles.y, z);

            if (_lerp >= 1)
            {
                _isLerp = false;

                _lerp = 0;
            }
        }
    }

    public void SetActualPosition(float position)
    {
        _actualPosition = Mathf.Abs(position);

        _mesh.localPosition = new Vector3(_actualPosition, _mesh.localPosition.y, _mesh.localPosition.z);

        _shadow.localPosition = new Vector3(_actualPosition, _shadow.localPosition.y, _shadow.localPosition.z);

        _shadow2.localPosition = new Vector3(_actualPosition, _shadow2.localPosition.y, _shadow2.localPosition.z);

        _camera.transform.localPosition = new Vector3(_actualPosition / 1.5f, _camera.transform.localPosition.y, _camera.transform.transform.localPosition.z);
    }

    private void EnableMove()
    {
        _isCanMove = true;
    }

    private void DisableMove()
    {
        _isCanMove = false;

        _actualPosition = 0;

        _moveRotationY = 0;

        _moveRotationX = 0;

        _cameraRotateZ = 0;

        _isLerp = true;
    }
}
