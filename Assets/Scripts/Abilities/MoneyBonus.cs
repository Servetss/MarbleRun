using UnityEngine;

public class MoneyBonus : Ability
{
    public override float Boost => 1 + ((_level + 3) * 0.1f);// Mathf.Ceil(100 * Mathf.Pow(1.07f, ));

    protected override void SetBoost()
    { 
    }
}
