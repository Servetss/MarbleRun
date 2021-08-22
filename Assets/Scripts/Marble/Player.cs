using Barmetler.RoadSystem;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    private RoadMover _roadMover;

    public LevelInfo LevelInfo { get; private set; }

    private void Awake()
    {
        LevelInfo = new LevelInfo();

        _roadMover = GetComponent<RoadMover>();
    }

    private void Start()
    {
        _roadMover.SubscribeOnFinish(TrackFinish);
    }

    public void LevelStart()
    {
        Road road = _levelPreparer.SelecetedRoad;
        
        _roadMover.SetRoad(road.GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(road.transform)).ToArray());
    }

    public void LevelFinish()
    {
        LevelInfo.AddLevel();
    }

    public void AddClick()
    {
        LevelInfo.AddClick();
    }

    public void NextLevel()
    {
        LevelInfo.ResetCoins();
    }

    public void TrackFinish()
    {
        
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
