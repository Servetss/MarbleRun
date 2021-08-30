 using UnityEngine;

public class AccelerationBonus : Ability
{
    private Accelerator _accelerator;

    protected override void SetBoost()
    {
        _accelerator = Player.GetComponent<Accelerator>();

        _accelerator.SetAcceleration(Boost);
    }
}
