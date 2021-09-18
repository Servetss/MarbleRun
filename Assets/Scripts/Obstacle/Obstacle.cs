using UnityEngine;

public class Obstacle : MonoBehaviour, IObjectActivator
{
    [SerializeField] private Piece[] _pieces;

    [Range(0, 2)]
    [SerializeField] private float _animationSlider;

    [SerializeField] private GameObject _mesh;

    [SerializeField] private ParticleSystem _particleSystem;

    private BoxCollider _boxCollider;

    private bool _isDestroing;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();

        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].Init();
        }
    }

    private void Update()
    {
        if (_isDestroing)
        {
            _animationSlider += Time.deltaTime * 0.3f;

            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i].PieceAnimate(_animationSlider);
            }

            if (_animationSlider > 2)
            {
                _isDestroing = false;

                DestroingAnimationFinish();
            }
        }
    }

    public void DestroyObstacle()
    {
        _isDestroing = true;

        _boxCollider.enabled = false;

        _particleSystem.Play();
    }

    public void Activate()
    {
        PiecesBackToDefaultPosition();

        _animationSlider = 0;

        _boxCollider.enabled = true;

        _mesh.SetActive(true);
    }

    private void PiecesBackToDefaultPosition()
    {
        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].PieceAnimate(0);
        }
    }

    private void DestroingAnimationFinish()
    {
        _mesh.SetActive(false);
    }
}

[System.Serializable]
public struct Piece
{
    [SerializeField] private Transform _meshPiece;

    [SerializeField] private VectorAnimationCurve _vectorAnimationCurve;

    private Vector3 _defaultPosition;

    public void Init()
    {
        _defaultPosition = _meshPiece.localPosition;
    }

    public void PieceAnimate(float time)
    {
        _meshPiece.localPosition = new Vector3(_defaultPosition.x + _vectorAnimationCurve.CurveX.Evaluate(time), _vectorAnimationCurve.CurveY.Evaluate(time), _vectorAnimationCurve.CurveZ.Evaluate(time));
    }
}
