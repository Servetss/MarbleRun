using UnityEngine;

public class SplineBoostZoneClicker : IBoost
{
    private EventMachine _playerEventMachine;

    private RoadMover _roadMover;

    private Jump _jump;

    private bool _isTriggerStay;

    private int _clickCount;

    public void Init(MarbleImpulse marbleImpulse)
    {
        _roadMover = marbleImpulse.GetComponent<RoadMover>();

        _jump = marbleImpulse.GetComponent<Jump>();

        _playerEventMachine = marbleImpulse.GetComponent<EventMachine>();

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
        _jump.Impulse(_roadMover.Speed / 4);
    }
}
