using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameOverPanel _gameOverPanel;

    [SerializeField] private LevelEventZone _levelEventZone;

    [SerializeField] private MaterialShining _materialShining;

    [SerializeField] private Ability _moneyBoostAbility;

    public LevelEventZone LevelEventZone { get => _levelEventZone; }

    public void MarbleStop(Player player)
    {
        _materialShining.StartAnim(player.XZone.GetComponent<MeshRenderer>());

        player.PlayerEventMachine.FinishMethod();

        LevelFinish(player.LevelInfo);
    }

    private void LevelFinish(LevelInfo levelInfo)
    {
        Wallet.instance.AddMoney((levelInfo.CoinsGetOnTheLevel * levelInfo.Boost) * (int)_moneyBoostAbility.Boost);

        _gameOverPanel.GameOverUI(levelInfo);
    }
}
