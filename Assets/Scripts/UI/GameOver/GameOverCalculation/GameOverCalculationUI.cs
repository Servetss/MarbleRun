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

        int coinsGet = levelInfo.CoinsGetOnTheLevel * levelInfo.Boost;

        _coinReceive.text = coinsGet + "$";
    }

    public void ClosePanel()
    {
        _gameOverPanel.GoToNextPanel();
    }
}
