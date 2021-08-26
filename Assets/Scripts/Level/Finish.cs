using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameOverPanel _gameOverPanel;

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
