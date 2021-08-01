using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarbleImpulse))]
public class MarbleImpulseEditor : Editor
{
    private MarbleImpulse _marbleImpulse;

    private Transform _transform;

    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        _marbleImpulse = target as MarbleImpulse;

        _transform = _marbleImpulse.transform;

        _rigidbody = _marbleImpulse.GetComponent<Rigidbody>();
    }

    private void OnSceneGUI()
    {
        if (_marbleImpulse.Rigidbody != null)
        {
            ShowMarbleVelocity();
        }
    }

    private void ShowMarbleVelocity()
    {
        Debug.DrawLine(_marbleImpulse.transform.localPosition, _marbleImpulse.transform.localPosition + _marbleImpulse.Rigidbody.velocity.normalized * 2, Color.red);
    }
}
