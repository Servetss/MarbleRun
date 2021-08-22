using UnityEngine;

public interface IBoost
{
    void Init(MarbleImpulse marbleImpulse);

    bool BoostCondition();

    void Impulse(Rigidbody rigidbody, int velocity);

    void EnterTrigger();

    void ExiteTrigger();
}

[System.Serializable]
public struct BoostСharacteristic
{
    [SerializeField] private BoostType _boostType;

    [Range(1, 40)]
    [SerializeField] private int _impulse;

    public BoostType BoostType { get => _boostType; }

    public int Impulse { get => _impulse; }

    public IBoost GetSelectBoostType()
    {
        switch (BoostType)
        {
            case BoostType.TestBoost:
                return new TestBoost();
            case BoostType.BoostZonePhysics:
                return new BoostZoneClicker();
            case BoostType.BoostZoneSpline:
                return new SplineBoostZoneClicker();
        }

        return null;
    }
}