using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameOverPanel _gameOverPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            other.GetComponent<Player>().LevelFinish();

            LevelFinish(other.GetComponent<Player>().LevelInfo);
        }
    }

    private void LevelFinish(LevelInfo levelInfo)
    {
        _gameOverPanel.GameOverUI(levelInfo);
    }
}
