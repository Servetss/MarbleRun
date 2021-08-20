using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    [SerializeField] private GameObject[] _levels;

    public int LevelsCount => _levels.Length;

    public GameObject GetLevelByIndex(int index)
    {
        return _levels[index];
    }
}
