using System;
using UnityEngine;

public class SkinModel
{
    public Action SkinChange;

    private SkinContainer _skinContainer;

    private int _skinIndexToView;

    public SkinModel(SkinContainer skinContainer)
    {
        _skinContainer = skinContainer;
    }

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
        SkinChange?.Invoke();
    }

    public void BuySkin()
    {
        
    }
}
