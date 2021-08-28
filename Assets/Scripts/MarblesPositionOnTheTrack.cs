using System.Linq;
using UnityEngine;

public class MarblesPositionOnTheTrack : MonoBehaviour
{
    private LevelPreparer _levelPreparer;

    private Enemys _enemys;

    private Player _player;

    private RoadMover[] _roadMover;

    private void Awake()
    {
        _levelPreparer = GetComponent<LevelPreparer>();

        _player = _levelPreparer.PlayerMarble;

        _enemys = _levelPreparer.Enemys;
    }

    private void Start()
    {
        RoadMover[] enemyRoadMover = _enemys.GetEnemyRoadMover();

        _roadMover = new RoadMover[enemyRoadMover.Length + 1];

        _roadMover[0] = _player.GetComponent<RoadMover>();

        for (int i = 1; i < _roadMover.Length; i++)
        {
            _roadMover[i] = enemyRoadMover[i - 1];
        }
    }

    private void FixedUpdate()
    {
        MarbleSortByDistance();
    }

    private void MarbleSortByDistance()
    {
        var distances = _roadMover.OrderByDescending(u => u.Distance);

        int count = 1;

        foreach (RoadMover marble in distances)
        {
            marble.PositionOnTheTrackView.SetPosition(count);

            count++;
        }

        count = 1;
    }
}
