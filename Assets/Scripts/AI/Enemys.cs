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
}
