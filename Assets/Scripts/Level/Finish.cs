using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameOverPanel _gameOverPanel;

    [SerializeField] private PlayerEventMachine _playerEventMachine;

    public void MarbleStop(Player player)
    {
        _playerEventMachine.FinishMethod();

        LevelFinish(player.LevelInfo);
    }

    private void LevelFinish(LevelInfo levelInfo)
    {
        _gameOverPanel.GameOverUI(levelInfo);
    }
}
