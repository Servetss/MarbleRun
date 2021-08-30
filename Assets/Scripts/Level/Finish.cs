using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameOverPanel _gameOverPanel;

    [SerializeField] private LevelEventZone _levelEventZone;

    public LevelEventZone LevelEventZone { get => _levelEventZone; }

    public void MarbleStop(Player player)
    {
        player.PlayerEventMachine.FinishMethod();

        LevelFinish(player.LevelInfo);
    }

    private void LevelFinish(LevelInfo levelInfo)
    {
        _gameOverPanel.GameOverUI(levelInfo);
    }
}
