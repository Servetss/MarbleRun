using System;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] private Transform _playerStartPosition;

    [SerializeField] private Transform[] _marbleStartPositions;

    public void SetMarbelsToPosition(Transform[] marbles)
    {
        if (marbles.Length > _marbleStartPositions.Length)
            throw new ArgumentException();

        for (int i = 0; i < marbles.Length; i++)
        {
            marbles[i].position = _marbleStartPositions[i].position;
        }
    }

    public void SetPlayerPosition(Transform player)
    {
        player.position = _playerStartPosition.position;
    }
}
