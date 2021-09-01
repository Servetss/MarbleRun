using UnityEngine;

public class MeshTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private MarbleImpulse _marbleImpulse;

    private void OnTriggerEnter(Collider other)
    {
        _player.OnPlayerMeshTriggerEnter(other);

        _marbleImpulse.OnPlayerMeshTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _marbleImpulse.OnPlayerMeshTriggerExit(other);
    }
}
