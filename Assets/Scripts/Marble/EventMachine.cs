using Firebase.Analytics;
using System;
using UnityEngine;

public enum PlayerState { Idle, RoadRide, BoostZone, Jump}
public class EventMachine : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private PlayerState _playerState;

    private Action RoadStart;

    private Action RoadEnd;

    private Action BoostZoneStart;

    private Action BoostZoneFinish;

    private Action Finish;

    private Action MoveToNextLevel;

    public PlayerState PlayerState { get => _playerState; }

    #region Normal level
    public void SubscribeOnRoadStartStart(Action method)
    {
        RoadStart += method;
    }

    public void SubscribeOnRoadEnd(Action method)
    {
        RoadEnd += method;
    }
    #endregion

    #region Boost zone
    public void SubscribeOnBoostZoneStart(Action method)
    {
        BoostZoneStart += method;
    }

    public void SubscribeOnBoostZoneFinish(Action method)
    {
        BoostZoneFinish += method;
    }
    #endregion

    #region Last stage
    public void SubscribeOnFinish(Action method)
    {
        Finish += method;
    }

    public void SubscribeOnMoveToNextLevel(Action method)
    {
        MoveToNextLevel += method;
    }
    #endregion

    public void RoadStartMethod()
    {
        _playerState = PlayerState.RoadRide;

        RoadStart?.Invoke();
    }

    public void RoadEndMethod()
    {
        RoadEnd?.Invoke();
    }

    public void BoostZoneStartMethod()
    {
        _playerState = PlayerState.BoostZone;

        BoostZoneStart?.Invoke();
    }

    public void BoostZoneFinishMethod()
    {
        _playerState = PlayerState.Jump;

        BoostZoneFinish?.Invoke();
    }

    public void FinishMethod()
    {
        _playerState = PlayerState.Idle;

        Finish?.Invoke();
    }

    public void NextLevelMethod()
    {
        MoveToNextLevel?.Invoke();
    }
}
