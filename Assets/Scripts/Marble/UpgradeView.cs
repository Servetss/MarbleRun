using UnityEngine;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private WaitCustom _waitCustom;

    [SerializeField] private ParticleForAbility[] _particleForAbilities;

    private BounceAnimation _bounceAnimation;

    private void Awake()
    {
        for (int i = 0; i < _particleForAbilities.Length; i++)
        {
            _particleForAbilities[i].OnAwake(transform.GetChild(0));
        }
        
        _bounceAnimation = new BounceAnimation(transform.GetChild(0));

        _bounceAnimation.SetAmplitude(0.2f);
    }
}

[System.Serializable]
public class ParticleForAbility
{
    public Ability Ability;

    public ParticleSystem ParticleSystem;

    private Transform _item;

    public void OnAwake(Transform item)
    {
        _item = item;

        Ability.BoostBuy += OnBoostClick;
    }

    private void OnBoostClick()
    {
        OnParticlePlay(ParticleSystem);
    }

    private void OnParticlePlay(ParticleSystem particleSystem)
    {
        particleSystem.transform.position = _item.position;

        particleSystem.Play();
    }
}