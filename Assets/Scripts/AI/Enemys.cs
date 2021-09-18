using UnityEngine;

public class Enemys : MonoBehaviour
{
    [SerializeField] private AI[] _enemys;

    public void StartRoad()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            _enemys[i].StartRoad();
        }
    }

    public void NextLevel()
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            _enemys[i].MoveToNextLevel();
        }
    }

    public RoadMover[] GetEnemyRoadMover()
    {
        RoadMover[] roadMovers = new RoadMover[_enemys.Length];

        for (int i = 0; i < _enemys.Length; i++)
        {
            roadMovers[i] = _enemys[i].GetComponent<RoadMover>();
        }

        return roadMovers;
    }

    public Transform[] GetEnemiesTransform()
    {
        Transform[] transforms = new Transform[_enemys.Length];

        for (int i = 0; i < _enemys.Length; i++)
        {
            transforms[i] = _enemys[i].transform;
        }

        return transforms;
    }

    public void SetNextMaxSpeedForEnemies(int level)
    {
        for (int i = 0; i < _enemys.Length; i++)
        {
            _enemys[i].SetNewMaxSpeed(level);
        }
    }
}
