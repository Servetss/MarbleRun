using UnityEngine;

public class AbilitySetup : MonoBehaviour
{
    [SerializeField] private AbilityPare[] _abilityPare;

    private AbilityPresenter[] _abilityPresenters;

    private void Awake()
    {
        _abilityPresenters = new AbilityPresenter[_abilityPare.Length];

        for (int i = 0; i < _abilityPare.Length; i++)
        {
            _abilityPresenters[i] = new AbilityPresenter(_abilityPare[i].Ability, _abilityPare[i].AbilityView);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _abilityPresenters.Length; i++)
        {
            _abilityPresenters[i].Enable();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _abilityPresenters.Length; i++)
        {
            _abilityPresenters[i].Disable();
        }
    }
}

[System.Serializable]
public class AbilityPare
{
    [SerializeField] private AbilityView _abilityView;

    [SerializeField] private Ability _ability;

    public AbilityView AbilityView { get => _abilityView; }

    public Ability Ability { get => _ability; }
}