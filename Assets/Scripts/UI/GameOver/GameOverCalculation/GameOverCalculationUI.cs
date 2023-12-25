using TMPro;
using UnityEngine;

public class GameOverCalculationUI : MonoBehaviour, IGameOverPanels
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private TextMeshProUGUI _coinReceive;

    [SerializeField] private TextMeshProUGUI _playerPlace;

    [SerializeField] private Ability _moneyBoostAbility;

    [SerializeField] private PositionOnTheTrackView _playerPositionOnTheTrack;

    private GameOverPanel _gameOverPanel;

    public void Init(GameOverPanel gameOverPanel)
    {
        _gameOverPanel = gameOverPanel;
    }

    public void OpenPanel(LevelInfo levelInfo)
    {
        _levelText.text = "Level: " + levelInfo.PlayerLevel.ToString();

        _playerPlace.text = NumberParser.NumberToPositionText(_playerPositionOnTheTrack.PositionNum) + " place";

        int coinsGet = levelInfo.CoinsGetOnTheLevel * levelInfo.Boost;

        _coinReceive.text = (coinsGet * _moneyBoostAbility.Boost) + "$";
    }

    public void ClosePanel()
    {
        _gameOverPanel.GoToNextPanel();
    }
}
