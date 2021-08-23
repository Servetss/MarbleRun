using System;
using UnityEngine;

public class PlayerEventMachine : MonoBehaviour
{
    private Action RoadStart;

    private Action NormalLevelFinish;

    private Action BoostZoneStart;

    private Action BoostZoneFinish;

    private Action RoadEnd;

    private Action Finish;

    private Action MoveToNextLevel;

    #region Normal level
    public void SubscribeOnRoadStartStart(Action method)
    {
        RoadStart += method;
    }

    public void SubscribeOnNormalLevelEnd(Action method)
    {
        NormalLevelFinish += method;
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

    #region Jump
    public void SubscribeOnRoadEnd(Action method)
    {
        RoadEnd += method;
    }

    public void SubscribeOnFinish(Action method)
    {
        Finish += method;
    }
    #endregion

    public void SubscribeOnMoveToNextLevel(Action method)
    {
        MoveToNextLevel += method;
    }

    public void RoadStartMethod()
    {
        RoadStart?.Invoke();
    }

    public void NormalLevelFinishMethod()
    {
        NormalLevelFinish?.Invoke();
    }

    public void BoostZoneStartMethod()
    {
        NormalLevelFinishMethod();

        BoostZoneStart?.Invoke();
    }

    public void BoostZoneFinishMethod()
    {
        BoostZoneFinish?.Invoke();
    }

    public void RoadEndMethod()
    {
        BoostZoneFinishMethod();

        RoadEnd?.Invoke();
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
