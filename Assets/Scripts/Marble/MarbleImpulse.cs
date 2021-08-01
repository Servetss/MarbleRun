using UnityEngine;

public enum BoostType { TestBoost, ZoneBoost}

public class MarbleImpulse : MonoBehaviour
{
    private IBoost _boost;

    [SerializeField] private BoostСharacteristic _boostСharacteristic;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        
        _boost = _boostСharacteristic.GetSelectBoostType();
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
        _boost.EnterTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        _boost.ExiteTrigger();
    }
}