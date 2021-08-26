using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private EventMachine _eventMachine;

    private RoadMover _roadMover;

    private void Awake()
    {
        LevelInfo = new LevelInfo();

        _roadMover = GetComponent<RoadMover>();
    }

    public EventMachine PlayerEventMachine { get => _eventMachine; }

    public LevelInfo LevelInfo { get; private set; }

    private void Start()
    {
        PlayerEventMachine.SubscribeOnFinish(TrackFinish);
    }

    private void LevelStart()
    {
        PlayerEventMachine.RoadStartMethod();

        PlayerEventMachine.SubscribeOnMoveToNextLevel(NextLevel);
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
