using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;

    [SerializeField] private Transform _finishPosition;

    public Vector3 StartPosition { get => _startPosition.position; }

    public Vector3 FinishPosition { get => _finishPosition.position; }

    public void SetPlayerToStartPosition(Transform player)
    {
        player.position = _startPosition.position;
    }

    public void SetFinishZoneToFinishPosition(Transform finishZone)
    {
        finishZone.position = _finishPosition.position;
    }
}
