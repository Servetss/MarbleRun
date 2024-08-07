﻿using Firebase.Analytics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private MainMenuPanel _mainMenuPanel;

    [SerializeField] private BoostView _boostView;

    [SerializeField] private Animator _gameCanvasAnimator;

    private Accelerator _accelerator;

    private xZone _selectedXZone;

    private EventMachine _eventMachine;

    private Animator _animator;

    private void Awake()
    {
        Time.timeScale = 10;

        LevelInfo = new LevelInfo();

        _animator = GetComponent<Animator>();

        _eventMachine = GetComponent<EventMachine>();

        _accelerator = GetComponent<Accelerator>();

        _eventMachine.SubscribeOnBoostZoneStart(EnableAnimator);

        _eventMachine.SubscribeOnRoadStartStart(DisableAnimator);

        _eventMachine.SubscribeOnRoadStartStart(SlideTutorial);

        _eventMachine.SubscribeOnBoostZoneStart(ShowBoostZoneImage);

        _eventMachine.SubscribeOnBoostZoneFinish(HideBoostZone);

        Application.targetFrameRate = 60;
    }

    public LevelInfo LevelInfo { get; private set; }

    public EventMachine PlayerEventMachine { get => _eventMachine; }

    public xZone XZone { get => _selectedXZone; }

    public BoostView BoostView { get => _boostView; }

    private void Start()
    {
        PlayerEventMachine?.SubscribeOnFinish(TrackFinish);

        PlayerEventMachine?.SubscribeOnMoveToNextLevel(NextLevel);

        PlayerEventMachine?.SubscribeOnRoadStartStart(() => {LevelInfo.IsWin = false; });
    }

    private void LevelStart()
    {
        FirebaseAnalytics.LogEvent("Race_Start", "level_start_count", (_levelPreparer.LevelCount + 1));

        PlayerEventMachine.RoadStartMethod();
    }

    private void TrackFinish()
    {
        if(LevelInfo.IsWin)
            LevelInfo.AddLevel();
    }

    public void NextLevel()
    {
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
    public void SkinPanelActive()
    {
        _mainMenuPanel.OpenSkinPanel();
    }

    public void MainMenuActive()
    {
        _mainMenuPanel.OpenMainMenuPanel();
    }
    #endregion

    #region Game panel UI Canvas
    public void SlideTutorial()
    {
        _gameCanvasAnimator.SetTrigger("SlideShow");
    }

    public void ShowBoostZoneImage()
    {
        _gameCanvasAnimator.SetBool("IsBoostZone", true);

        _gameCanvasAnimator.SetTrigger("BoostZoneTrigger");
    }

    public void HideBoostZone()
    {
        _gameCanvasAnimator.SetBool("IsBoostZone", false);
    }
    #endregion

    public void OnPlayerMeshTriggerEnter(Collider other)
    {
        if (other.GetComponent<Obstacle>())
        {
            other.GetComponent<Obstacle>().DestroyObstacle();

            SoundManager.Instance.Vibration();

            SoundManager.Instance.OnObstacleCrash(transform.position);

            _accelerator.SubtractSpeed(0.5f);
        }
        else if (other.GetComponent<Coin>())
        {
            other.GetComponent<Coin>().PickUp();

            SoundManager.Instance.OnCoinPickUp();

            LevelInfo.AddCoin();
        }
        else if (other.GetComponent<xZone>())
        {
            _selectedXZone = other.GetComponent<xZone>();

            LevelInfo.SetBoost(other.GetComponent<xZone>().Boost);
        }
    }
}
