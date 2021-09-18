using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameOverPanel _gameOverPanel;

    [SerializeField] private LevelEventZone _levelEventZone;

    [SerializeField] private MaterialShining _materialShining;

    [SerializeField] private Ability _moneyBoostAbility;

    [SerializeField] private EventMachine _playerEventMachine;

    [SerializeField] private Player _player;

    [SerializeField] private ParticleSystem[] _finishParticle;

    public LevelEventZone LevelEventZone { get => _levelEventZone; }

    private void Start()
    {
        _playerEventMachine?.SubscribeOnBoostZoneFinish(FinishFirewarkStart);
    }

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

    private void FinishFirewarkStart()
    {
        if (_player.LevelInfo.IsWin)
        {
            SoundManager.Instance.FinishFirewark();

            for (int i = 0; i < _finishParticle.Length; i++)
            {
                _finishParticle[i].Play();
            }
        }
    }
}
