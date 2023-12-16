using Firebase.Analytics;
using System;
using UnityEngine;

public class EventMachine : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    private Action RoadStart;

    private Action RoadEnd;

    private Action BoostZoneStart;

    private Action BoostZoneFinish;

    private Action Finish;

    private Action MoveToNextLevel;

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

        RoadStart?.Invoke();
    }

    public void RoadEndMethod()
    {
        RoadEnd?.Invoke();
    }

    public void BoostZoneStartMethod()
    {
        BoostZoneStart?.Invoke();
    }

    public void BoostZoneFinishMethod()
    {
        BoostZoneFinish?.Invoke();
    }

    public void FinishMethod()
    {
        Finish?.Invoke();
    }

    public void NextLevelMethod()
    {
        MoveToNextLevel?.Invoke();
    }
}
