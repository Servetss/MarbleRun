using System;
using UnityEngine;

public class Ability : MonoBehaviour
{
    private const string AbilitySave = "AbilitySave";

    [SerializeField] private Player _player;

    [SerializeField] protected int _level;

    [Header("Boost")]
    [SerializeField] protected float _startBoost;

    [SerializeField] protected float _increaseBoost;

    [Header("Cost")]
    [SerializeField] protected int _startPrice;

    [SerializeField] protected int _increaseCost;

    public Action BoostBuy;

    public Action BoostCanNotBuy;

    private void Awake()
    {
        Load();
    }

    private void Start()
    {
        BoostBuy += SetBoost;
        
        SetBoost();
    }

    protected virtual void OnStart()
    {
        
    }

    public int Cost => (int)Mathf.Ceil(_startPrice * Mathf.Pow(1.07f, _level));

    public virtual float Boost => _startBoost + (_level * _increaseBoost);

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
        else
        {
            BoostCanNotBuy?.Invoke();
        }
    }

    protected void Buy()
    {
        Wallet.instance.SpendMoney(Cost);
        
        _level++;

        Save();
    }

    #region Save\Load
    public void Save()
    {
        PlayerPrefs.SetInt(AbilitySave + gameObject.name, _level);
    }

    public void Load()
    {
        _level = PlayerPrefs.GetInt(AbilitySave + gameObject.name);

        BoostBuy?.Invoke();
    }

    [ContextMenu("Reset save")]
    public void ResetSave()
    {
        PlayerPrefs.SetInt(AbilitySave + gameObject.name, 0);
    }
    #endregion
}
