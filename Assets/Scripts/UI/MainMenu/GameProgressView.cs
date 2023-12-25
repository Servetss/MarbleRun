using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressView : MonoBehaviour
{
    private const string SaveGameView = "GameProgressView ";

    [SerializeField] private Player _player;

    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private GameObject[] _checkImages;

    private int _checedLevels;

    private void Awake()
    {
        //ResetLoad();

        Load();
    }

    private void Start()
    {
        ShowLevel();
    }

    public void CompleteLevel()
    {
        _checedLevels++;

        Save();

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

    #region Save \ Load
    public void Save()
    {
        PlayerPrefs.SetInt(SaveGameView, _checedLevels + 1);
    }

    public void Load()
    {
        _checedLevels = PlayerPrefs.GetInt(SaveGameView) - 1;
    }

    public void ResetLoad()
    {
        PlayerPrefs.SetInt(SaveGameView, 0);
    }
    #endregion
}
