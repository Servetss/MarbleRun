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
    }

    public void Disable()
    {
        _model.SkinChange -= OnSkinChanged;

        _view.ClickOnNextSkin -= OnClickNext;
        _view.ClickOnPreviousSkin -= OnClicPrevious;
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
