using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private MainMenuPanel _mainMenuPanel;

    private EventMachine _eventMachine;

    private RoadMover _roadMover;

    private Animator _animator;

    private void Awake()
    {
        LevelInfo = new LevelInfo();

        _roadMover = GetComponent<RoadMover>();

        _animator = GetComponent<Animator>();

        _eventMachine = GetComponent<EventMachine>();

        _eventMachine.SubscribeOnBoostZoneStart(EnableAnimator);

        _eventMachine.SubscribeOnRoadStartStart(DisableAnimator);
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

    public void EnableAnimator()
    {
        _animator.enabled = true;
    }

    public void DisableAnimator()
    {
        _animator.enabled = false;
    }

    #region Skin panel animation
    public void SkinModeOn()
    {
        _animator.SetBool("ChangeSkin", true);
    }

    public void SkinModeOff()
    {
        _animator.SetBool("ChangeSkin", false);
    }

    public void SkinPanelActive()
    {
        _mainMenuPanel.OpenSkinPanel();
    }

    public void MainMenuActive()
    {
        _mainMenuPanel.OpenMainMenuPanel();
    }
    #endregion

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Coin>())
        {
            other.GetComponent<Coin>().PickUp();

            LevelInfo.AddCoin();
        }
    }
}
