using UnityEngine;
using UnityEngine.UI;

public class GameOverCalculationUI : MonoBehaviour, IGameOverPanels
{
    [SerializeField] private Text _levelText;

    [SerializeField] private Text _coinReceive;

    private GameOverPanel _gameOverPanel;

    public void Init(GameOverPanel gameOverPanel)
    {
        _gameOverPanel = gameOverPanel;
    }

    public void OpenPanel(LevelInfo levelInfo)
    {
        _levelText.text = "Level: " + levelInfo.PlayerLevel.ToString();

        _coinReceive.text = levelInfo.CoinsGetOnTheLevel.ToString() + "$";
    }

    public void ClosePanel()
    {
        _gameOverPanel.GoToNextPanel();
    }
}
