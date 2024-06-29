using TMPro;
using UnityEngine;

public class GameOverCalculationUI : MonoBehaviour, IGameOverPanels
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private TextMeshProUGUI _coinReceive;

    [SerializeField] private TextMeshProUGUI _playerPlace;

    [SerializeField] private MoneyBonus _moneyBoostAbility;

    [SerializeField] private PositionOnTheTrackView _playerPositionOnTheTrack;

    [Header("Level Completed")]
    [SerializeField] private GameObject _completeLevel;

    [SerializeField] private GameObject _failedLevel;

    private GameOverPanel _gameOverPanel;

    public void Init(GameOverPanel gameOverPanel)
    {
        _gameOverPanel = gameOverPanel;
    }

    public void OpenPanel(LevelInfo levelInfo)
    {
        _levelText.text = "LEVEL " + levelInfo.PlayerLevel.ToString();

        _playerPlace.text = NumberParser.NumberToPositionText(_playerPositionOnTheTrack.PositionNum) + " PLACE";
        
        int coinsGet = (int)((levelInfo.CoinsGetOnTheLevel * levelInfo.Boost) * _moneyBoostAbility.Boost);

        _coinReceive.text = coinsGet + "$";

        if (_playerPositionOnTheTrack.PositionNum == 1)
        {
            _completeLevel.SetActive(true);
            _failedLevel.SetActive(false);
        }
        else
        {
            _completeLevel.SetActive(false);
            _failedLevel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        _gameOverPanel.GoToNextPanel();
    }
}
