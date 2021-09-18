using UnityEngine;

public class SkinPresenter
{
    private SkinChangerView _view;

    private SkinModel _model;

    public SkinPresenter(SkinChangerView skinChangerView, SkinModel skinModel)
    {
        _view = skinChangerView;

        _model = skinModel;
    }

    public void Enable()
    {
        _model.SkinChange += OnSkinChanged;

        _view.ClickOnNextSkin += OnClickNext;
        _view.ClickOnPreviousSkin += OnClicPrevious;

        _view.ClickOnBuySkin += OnSkinBuy;

        _view.ClickOnSelectedSkin += OnSkinChanged;

        _model.SkinChange?.Invoke();
    }

    public void Disable()
    {
        _model.SkinChange -= OnSkinChanged;

        _view.ClickOnNextSkin -= OnClickNext;
        _view.ClickOnPreviousSkin -= OnClicPrevious;

        _view.ClickOnBuySkin -= OnSkinBuy;

        _view.ClickOnSelectedSkin -= OnSkinChanged;
    }

    private void OnSkinBuy()
    {
        _model.BuySkin();

        OnSkinChanged();
    }

    private void OnSkinChanged()
    {
        _view.SetSkin(_model.SelectedSkin);
    }

    private void OnClickNext()
    {
        _model.MoveToNextSkin();
    }

    private void OnClicPrevious()
    {
        _model.MoveToPreviousSkin();
    }
}
