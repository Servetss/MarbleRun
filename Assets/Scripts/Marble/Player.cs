using Barmetler.RoadSystem;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    private RoadMover _roadMover;

    private PlayerEventMachine _playerEventMachine;
    public LevelInfo LevelInfo { get; private set; }

    private void Awake()
    {
        LevelInfo = new LevelInfo();

        _roadMover = GetComponent<RoadMover>();

        _playerEventMachine = GetComponent<PlayerEventMachine>();
    }

    private void Start()
    {
        _playerEventMachine.SubscribeOnFinish(TrackFinish);
    }

    private void LevelStart()
    {
        _playerEventMachine.RoadStartMethod();

        _playerEventMachine.SubscribeOnMoveToNextLevel(NextLevel);
    }

    private void TrackFinish()
    {
        LevelInfo.AddLevel();
    }

    public void NextLevel()
    {
        GetComponent<Rigidbody>().isKinematic = true;

        LevelInfo.ResetCoins();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Coin>())
        {
            other.GetComponent<Coin>().PickUp();

            LevelInfo.AddCoin();
        }
    }
}
