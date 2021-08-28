using UnityEngine;

public class Ability
{
    [SerializeField] private int _level;

    [SerializeField] private int _cost;

    [SerializeField] private int _increaseCost;

    protected virtual void OnClick()
    {
        
    }

    protected bool IsCanBuy => true;

    protected void Buy()
    {
        _level++;
    }
}
