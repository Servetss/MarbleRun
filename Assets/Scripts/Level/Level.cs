using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;

    [SerializeField] private Transform _finishPosition;

    public Transform StartTransform { get => _startPosition; }

    public Transform FinishTransform { get => _finishPosition; }

    public void SetPlayerToStartPosition(Transform player)
    {
        player.position = _startPosition.position;
    }

    public void SetFinishZoneToFinishPosition(Transform finishZone)
    {
        finishZone.position = _finishPosition.position;
    }
}
