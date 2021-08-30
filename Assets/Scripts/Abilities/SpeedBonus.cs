using UnityEngine;

public class SpeedBonus : Ability
{
    private Accelerator _accelerator;

    protected override void SetBoost()
    {
        _accelerator = Player.GetComponent<Accelerator>();

        _accelerator.SetSpeedBoost(Boost);
    }
}
