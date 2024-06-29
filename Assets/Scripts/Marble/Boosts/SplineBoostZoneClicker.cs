using UnityEngine;

public class SplineBoostZoneClicker : IBoost
{
    private EventMachine _playerEventMachine;

    private RoadMover _roadMover;

    private BoostView _boostView;

    private Accelerator _accelerator;

    private Jump _jump;

    private bool _isTriggerStay;

    private int _clickCount;

    public void Init(MarbleImpulse marbleImpulse)
    {
        _roadMover = marbleImpulse.GetComponent<RoadMover>();

        _jump = marbleImpulse.GetComponent<Jump>();

        _playerEventMachine = marbleImpulse.GetComponent<EventMachine>();

        _playerEventMachine.SubscribeOnRoadEnd(JumpInTheEnd);

        _boostView = marbleImpulse.GetComponent<Player>().BoostView;

        _accelerator = marbleImpulse.GetComponent<Accelerator>();
    }

    public bool BoostCondition()
    {
        return _isTriggerStay && Input.GetMouseButtonDown(0);
    }

    public void Impulse(Rigidbody rigidbody, float velocity)
    {
        _clickCount++;
        
        _roadMover.Accelerate(velocity * Time.deltaTime);

        _boostView.SetFillAmount((int)_accelerator.MinimalSpeed, (int)_accelerator.MaximalSpeed, _roadMover.Speed);
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
