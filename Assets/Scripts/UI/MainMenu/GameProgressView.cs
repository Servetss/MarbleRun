using UnityEngine;
using UnityEngine.UI;

public class GameProgressView : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private Text _levelText;

    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private GameObject[] _checkImages;

    private EventMachine _eventMachine;

    private int _checedLevels;

    private void Awake()
    {
        _eventMachine = _player.GetComponent<EventMachine>();

        _checedLevels = -2;

        ShowLevel();
    }

    private void Start()
    {
        _eventMachine?.SubscribeOnMoveToNextLevel(CompleteLevel);
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
