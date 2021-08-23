using UnityEngine;

public enum BoostType { TestBoost, BoostZonePhysics, BoostZoneSpline}

public class MarbleImpulse : MonoBehaviour
{
    private IBoost _boost;

    [SerializeField] private BoostСharacteristic _boostСharacteristic;

    private PlayerEventMachine _playerEventMachine;

    private Animator _animator;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();

        _playerEventMachine = GetComponent<PlayerEventMachine>();

        _animator = GetComponent<Animator>();

        _boost = _boostСharacteristic.GetSelectBoostType();

        _boost.Init(this);
    }

    public Rigidbody Rigidbody { get; private set; }

    private void Update()
    {
        if (_boost.BoostCondition())
        {
            _boost.Impulse(Rigidbody, _boostСharacteristic.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BoostZone")
        {
            _animator.SetBool("IsBoostZone", true);

            _playerEventMachine.BoostZoneStartMethod();

            _boost.EnterTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BoostZone")
        {
            _animator.SetBool("IsBoostZone", false);

            _playerEventMachine.BoostZoneFinishMethod();

            _boost.ExiteTrigger();
        }
    }
}