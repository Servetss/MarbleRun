using System;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private const string AbilitySave = "AbilitySave";

    [SerializeField] private Player _player;

    [SerializeField] private int _level;

    [Header("Boost")]
    [SerializeField] private float _startBoost;

    [SerializeField] private float _increaseBoost;

    [Header("Coast")]
    [SerializeField] private int _startCost;

    [SerializeField] private int _increaseCost;

    public Action BoostBuy;

    private void Awake()
    {
        BoostBuy += SetBoost;

        Load();
    }

    public int Cost => _startCost + (_level * _increaseCost);

    public float Boost => _startBoost + (_level * _increaseBoost);

    public int Level { get => _level; }

    protected Player Player { get => _player; }

    protected bool IsCanBuy => true;

    protected virtual void SetBoost()
    {

    }

    public void OnClick()
    {
        if (Wallet.instance.Value >= Cost)
        {
            Buy();

            BoostBuy?.Invoke();
        }
    }

    protected void Buy()
    {
        Wallet.instance.SpendMoney(Cost);

        _level++;
    }

    #region Save\Load
    public void Save()
    {
        
    }

    public void Load()
    {
        BoostBuy?.Invoke();
    }
    #endregion
}
