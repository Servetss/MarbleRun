using UnityEngine;
using UnityEngine.UI;

public class GameProgressView : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private Text _levelText;

    [SerializeField] private GameObject[] _checkImages;

    private int _checedLevels;

    private void Awake()
    {
        _checedLevels = -1;

        ShowLevel();
    }

    public void CompleteLevel()
    {
        _checedLevels++;

        ShowLevel();
    }

    public void ShowLevel()
    {
        if (_checedLevels >= _checkImages.Length - 1)
            _checedLevels = -1;

        _levelText.text = "Level: " + (_levelPreparer.LevelCount + 1).ToString();

        for (int i = 0; i < _checkImages.Length; i++)
        {
            _checkImages[i].SetActive(i <=_checedLevels);
        }
    }
}
