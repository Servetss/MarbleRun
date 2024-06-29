using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private const string WalletSave = "WalletSave";

    public static Wallet instance;

    public Action MoneyChange;

    private void Awake()
    {
        instance = this;
    }

    public int Value { get; private set; }

    private void Start()
    {
        SubscribeOnMoneyChange(Save);

        Invoke("Load", Time.deltaTime);
        
        OnFirstStart();
    }

    private void OnFirstStart()
    {
        if (PlayerPrefs.GetInt("IsFirstStart") == 0)
        {
            PlayerPrefs.SetInt("IsFirstStart", 1);

            AddMoney(90);
        }
    }

    [ContextMenu("Add some money")]
    public void AddSomeMoney()
    {
        AddMoney(1200);
    }

    public void AddMoney(int value)
    {
        Value += value;

        MoneyChange?.Invoke();
    }

    public void SpendMoney(int value)
    {
        Value -= value;

        MoneyChange?.Invoke();
    }

    public void MoneyRefresh()
    {
        MoneyChange?.Invoke();
    }

    #region Subscribe Events
    public void SubscribeOnMoneyChange(Action method)
    {
        MoneyChange += method;
    }
    #endregion

    #region Save\Load
    private void Save()
    {
        PlayerPrefs.SetInt(WalletSave, Value);
    }

    private void Load()
    {
        Value = PlayerPrefs.GetInt(WalletSave);

        MoneyRefresh();
    }
    #endregion
}
