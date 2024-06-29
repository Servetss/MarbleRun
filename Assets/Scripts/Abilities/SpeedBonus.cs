using UnityEngine;

public class SpeedBonus : Ability
{
    private Accelerator _accelerator;

    public override float Boost => SpeedBalance.CalculatePlayerSppedByLevel(_level);

    private void Awake()
    {
        SetBoost();
    }

    protected override void SetBoost()
    {
        _accelerator = Player.GetComponent<Accelerator>();

        _accelerator.SetSpeedBoost(Boost);
    }
}
