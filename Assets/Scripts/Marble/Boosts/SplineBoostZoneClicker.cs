using UnityEngine;

public class SplineBoostZoneClicker : IBoost
{
    private RoadMover _roadMover;

    private PlayerEventMachine _playerEventMachine;

    private Jump _jump;

    private bool _isTriggerStay;

    private int _clickCount;

    public void Init(MarbleImpulse marbleImpulse)
    {
        _roadMover = marbleImpulse.GetComponent<RoadMover>();

        _playerEventMachine = marbleImpulse.GetComponent<PlayerEventMachine>();

        _jump = marbleImpulse.GetComponent<Jump>();

        _playerEventMachine.SubscribeOnRoadEnd(JumpInTheEnd);
    }

    public bool BoostCondition()
    {
        return _isTriggerStay && Input.GetMouseButtonDown(0);
    }

    public void Impulse(Rigidbody rigidbody, int velocity)
    {
        _clickCount++;

        _roadMover.Accelerate(3);
    }

    public void EnterTrigger()
    {
        _isTriggerStay = true;
    }

    public void ExiteTrigger()
    {
        _isTriggerStay = false;
    }

    private void JumpInTheEnd()
    {
        _jump.Impulse(_roadMover.Speed);
    }
}
