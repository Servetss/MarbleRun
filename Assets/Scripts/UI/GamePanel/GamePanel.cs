using UnityEngine;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private GameObject _backGround;

    [SerializeField] private EventMachine _playerEvent;

    private void Start()
    {
        _playerEvent?.SubscribeOnRoadStartStart(EnableGameBackground);

        _playerEvent?.SubscribeOnMoveToNextLevel(DisableGameBackground);

        _backGround.SetActive(false);
    }

    public void EnableGameBackground()
    {
        _backGround.SetActive(true);
    }

    public void DisableGameBackground()
    {
        _backGround.SetActive(false);
    }
}
