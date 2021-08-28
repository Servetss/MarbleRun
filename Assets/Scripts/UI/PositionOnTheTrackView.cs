using UnityEngine;
using UnityEngine.UI;

public class PositionOnTheTrackView : MonoBehaviour
{
    [SerializeField] private Text _numberPosition;

    private EventMachine _eventMachine;

    private bool _isCanChange;

    private void Awake()
    {
        _eventMachine = GetComponent<EventMachine>();
    }

    private void Start()
    {
        _eventMachine?.SubscribeOnRoadStartStart(EnablesPositionChanging);

        _eventMachine?.SubscribeOnRoadEnd(DisablePositionChanging);
    }

    public void SetPosition(int positionNum)
    {
        if(_isCanChange)
            _numberPosition.text = positionNum.ToString();
    }

    public void EnablesPositionChanging()
    {
        _isCanChange = true;
    }

    public void DisablePositionChanging()
    {
        _isCanChange = false;
    }
}
