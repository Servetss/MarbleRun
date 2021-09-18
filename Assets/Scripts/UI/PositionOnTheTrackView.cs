using UnityEngine;
using UnityEngine.UI;

public class PositionOnTheTrackView : MonoBehaviour
{
    [SerializeField] private Text _numberPosition;

    private EventMachine _eventMachine;

    private Player _player;

    private void Awake()
    {
        _eventMachine = GetComponent<EventMachine>();

        _player = GetComponent<Player>();
    }

    public bool IsCanChange { get; private set; }

    public int PositionNum { get; private set; }

    private void Start()
    {
        IsCanChange = true;

        _eventMachine?.SubscribeOnMoveToNextLevel(EnablesPositionChanging);

        _eventMachine?.SubscribeOnBoostZoneFinish(DisablePositionChanging);
    }

    public void SetPosition(int positionNum)
    {
        if (IsCanChange)
        {
            PositionNum = positionNum;

            if(_player != null)
                _player.LevelInfo.IsWin = positionNum == 1;

            _numberPosition.text = NumberParser.NumberToPositionText(positionNum);
        }
    }

    public void EnablesPositionChanging()
    {
        IsCanChange = true;
    }

    public void DisablePositionChanging()
    {
        IsCanChange = false;
    }
}
