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

        Wallet.instance.SubscribeOnMoneyChange(OnMoneyChanged);

        OnAbilityChanged();
    }

    public void Disable()
    {
        _model.BoostBuy -= OnAbilityChanged;
        _view.Click -= OnViewClick;
    }

    private void OnAbilityChanged()
    {
        _view.SetView(_model.Level, _model.Cost, _model.Boost);
    }

    private void OnViewClick()
    {
        _model.OnClick();
    }

    private void OnMoneyChanged()
    {
        _view.WhenMoneyChange(_model.Cost);
    }
}
