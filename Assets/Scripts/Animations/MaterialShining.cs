using UnityEngine;

public class MaterialShining : MonoBehaviour
{
    [SerializeField] private EventMachine _playerEventmachine;

    [Range(0.01f, 1)]
    [SerializeField] private float _animationSpeed;

    [SerializeField] private Color _colorTo;

    private Color _colorFrom;

    private Material _material;

    private bool _isAnimated;

    private float _distance;

    private float _sin;

    private void Start()
    {
        _playerEventmachine?.SubscribeOnMoveToNextLevel(StopAnim);
    }

    private void Update()
    {
        if (_isAnimated)
        {
            _distance += Time.deltaTime * _animationSpeed;

            _sin = Mathf.Abs(Mathf.Sin(_distance * Mathf.PI));

            _material.SetColor("_Color", Color.Lerp(_colorFrom, _colorTo, _sin));
        }
    }

    public void StartAnim(MeshRenderer meshRenderer)
    {
        _material = meshRenderer.material;

        _colorFrom = _material.GetColor("_Color");

        _isAnimated = true;
    }

    public void StopAnim()
    {
        _isAnimated = false;

        _material?.SetColor("_Color", _colorFrom);
    }
}
