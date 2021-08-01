using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _followedtransform;

    private Vector3 _different;

    private void Awake()
    {
        _different =  transform.localPosition - _followedtransform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = _different + _followedtransform.localPosition;
    }
}
