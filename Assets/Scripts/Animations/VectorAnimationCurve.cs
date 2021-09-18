using UnityEngine;

[System.Serializable]
public struct VectorAnimationCurve
{
    [SerializeField] private AnimationCurve _curveX;

    [SerializeField] private AnimationCurve _curveY;

    [SerializeField] private AnimationCurve _curveZ;

    public AnimationCurve CurveX { get => _curveX; }

    public AnimationCurve CurveY { get => _curveY; }

    public AnimationCurve CurveZ { get => _curveZ; }
}
