using System;
using UnityEngine;

public class SkinModel
{
    private const string SkinModelSave = "SkinModelSave";

    private SkinContainer _skinContainer;

    private int _skinIndexToView;

    private int _selectedUnlocked;
    
    public Action SkinChange;

    public Action SkinBuy;

    public SkinModel(SkinContainer skinContainer)
    {
        _skinContainer = skinContainer;

        Load();

        SelectSkin();

        _skinContainer.OnShopBackClick += SelectSkin;
    }

    public SkinSO SkinOnPlayer { get; private set; }

    public SkinSO PreviewSelectedSkin { get; private set; }

    public SkinSO SkinInShopSave { get; private set; }

    public void MoveToNextSkin()
    {
        _skinIndexToView = _skinContainer.GetNextSkinIndex(_skinIndexToView);

        PreviewSelectedSkin = _skinContainer.GetSkinByIndex(_skinIndexToView);

        SelectSkin();
    }

    public void MoveToPreviousSkin()
    {
        _skinIndexToView = _skinContainer.GetPreviousSkinIndex(_skinIndexToView);

        PreviewSelectedSkin = _skinContainer.GetSkinByIndex(_skinIndexToView);

        SelectSkin();
    }

    public void SelectPreviousUnlocked()
    {
        PreviewSelectedSkin = _skinContainer.GetSkinByIndex(_selectedUnlocked);

        SkinOnPlayer = PreviewSelectedSkin;

        _skinIndexToView = _selectedUnlocked;
        
        SkinChange?.Invoke();

        Save();
    }

    public void SelectSkin()
    {
        SkinOnPlayer = PreviewSelectedSkin;

        if (SkinOnPlayer.IsUnlocked)
        {
            _selectedUnlocked = _skinIndexToView;

            Save();
        }
        
        SkinChange?.Invoke();
    }

    public void BuySkin()
    {
        if (Wallet.instance.Value >= PreviewSelectedSkin.SkinCost)
        {
            Wallet.instance.SpendMoney(PreviewSelectedSkin.SkinCost);
            
            _selectedUnlocked = _skinIndexToView;

            PreviewSelectedSkin.UnlockTheSkin();

            SkinOnPlayer = PreviewSelectedSkin;

            SkinChange?.Invoke();
        }
    }

    #region Save \ Load
    private void Save()
    {
        PlayerPrefs.SetInt(SkinModelSave, _skinIndexToView);
    }

    public void Load()
    {
        _skinIndexToView = PlayerPrefs.GetInt(SkinModelSave);

        PreviewSelectedSkin = _skinContainer.GetSkinByIndex(_skinIndexToView);
    }
    #endregion
}
