using UnityEngine;

public class SpeedBonus : Ability
{
    [Space]
    [SerializeField] private Enemys _enemys;

    private Accelerator _accelerator;

    protected override void SetBoost()
    {
        _accelerator = Player.GetComponent<Accelerator>();

        _accelerator.SetSpeedBoost(Boost);

        _enemys.SetNextMaxSpeedForEnemies(Level);
    }
}
