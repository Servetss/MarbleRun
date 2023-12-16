using UnityEngine;

public class ShaderAdapter : MonoBehaviour
{
    private Transform _camera;

    private Material _material;

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;

        _camera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        _material.SetVector("_CameraViewVector", _camera.localPosition);
    }
}