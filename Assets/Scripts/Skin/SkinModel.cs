using System;
using UnityEngine;

public class SkinModel
{
    private SkinContainer _skinContainer;

    private int _skinIndexToView;

    public Action SkinChange;

    public Action SkinBuy;

    public SkinModel(SkinContainer skinContainer)
    {
        _skinContainer = skinContainer;
    }

    public SkinSO SkinOnPlayer { get; private set; }

    public SkinSO SelectedSkin { get; private set; }

    public void MoveToNextSkin()
    {
        _skinIndexToView = _skinContainer.GetNextSkinIndex(_skinIndexToView);

        SelectedSkin = _skinContainer.GetSkinByIndex(_skinIndexToView);

        SelectSkin();
    }

    public void MoveToPreviousSkin()
    {
        _skinIndexToView = _skinContainer.GetPreviousSkinIndex(_skinIndexToView);

        SelectedSkin = _skinContainer.GetSkinByIndex(_skinIndexToView);

        SelectSkin();
    }

    public void SelectSkin()
    {
        SkinOnPlayer = SelectedSkin;

        SkinChange?.Invoke();
    }

    public void BuySkin()
    {
        if (Wallet.instance.Value >= SelectedSkin.SkinCost)
        {
            Wallet.instance.SpendMoney(SelectedSkin.SkinCost);

            SelectedSkin.UnlockTheSkin();

            SkinOnPlayer = SelectedSkin;

            SkinChange?.Invoke();
        }
    }
}
