using UnityEngine;

public enum DirectionMove { Up, Down, Right, Left }

public class UIHideAnimation : MonoBehaviour
{
    [SerializeField] private DirectionMove _hideDirection;

    [SerializeField] private float _distanceToMove;

    private AnimatedData _moveAnimation;

    private RectTransform _rectTransform;

    private Vector3 _startPosition;

    private Vector3 _targetPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _startPosition = _rectTransform.anchoredPosition;
        
        _targetPosition = _startPosition + (DirectionMoveToVector(_hideDirection) * _distanceToMove);
        
        _moveAnimation = new RectTransformAnimation(_rectTransform, _targetPosition);
    }

    private Vector3 DirectionMoveToVector(DirectionMove hideDirection)
    {
        switch (hideDirection)
        {
            case DirectionMove.Up:
                return Vector3.up;
            case DirectionMove.Down:
                return -Vector3.up;
            case DirectionMove.Right:
                return Vector3.right;
            case DirectionMove.Left:
                return -Vector3.right;
        }

        return Vector3.zero;
    }

    public void SetDefaultPositionShow()
    {
        _rectTransform.anchoredPosition = _startPosition;
    }

    public void StartHide()
    {
        WaitCustom.Instance.Play(_moveAnimation, 0.8f);
    }
}
