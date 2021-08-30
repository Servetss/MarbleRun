public class AbilityPresenter
{
    private Ability _model;

    private AbilityView _view;

    public AbilityPresenter(Ability model, AbilityView view)
    {
        _model = model;

        _view = view;
    }

    public void Enable()
    {
        _model.BoostBuy += OnAbilityChanged;
        _view.Click += OnViewClick;

        OnAbilityChanged();
    }

    public void Disable()
    {
        _view.Click -= _model.OnClick;
    }

    private void OnAbilityChanged()
    {
        _view.SetView(_model.Level, _model.Cost, _model.Boost);
    }

    private void OnViewClick()
    {
        _model.OnClick();
    }
}
