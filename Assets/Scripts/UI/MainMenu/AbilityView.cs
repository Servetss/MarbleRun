using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _boostText;

    [SerializeField] private TextMeshProUGUI _abilityCost;

    [SerializeField] private Transform _costImagePanel;

    [SerializeField] private Image _allScreenShine;

    [SerializeField] private Animator _marbleAnimator;

    [Header("Components to color change")]
    [SerializeField] private Image _buttonImage;

    [SerializeField] private Image _shineImage;

    [SerializeField] private GameObject _blockPanel; 
    
    [Header("Shine Data")]
    [SerializeField] private Transform _backShinePanel;

    [SerializeField] private Image _backShineImage;

    [Header("Animation")]
    [SerializeField] private float _cameraShakeSpeed;
    
    [SerializeField] private ButtonBounceAnimateParametres _onSuccessBuy;

    [SerializeField] private ButtonBounceAnimateParametres _onFailedBuy;

    [SerializeField] private ButtonBounceAnimateParametres _backShineAnimation; 

    private BounceAnimation _bounceAnimation;

    private AnimatedData _shineAnimation;

    private AnimatedData _upgradeAnimation;

    private AnimatedData _blockAnimation;

    private CameraShake _cameraShake;
    
    public Action Click;

    private void Awake()
    {
        _shineAnimation = new ShineAnimation(_allScreenShine.transform, _allScreenShine);

        _upgradeAnimation = new ColorChange(_shineImage, new Color(1, 1, 1, 0), new Color(0.6f, 0.9f, 1, 0.6f));

        _blockAnimation = new ColorChange(_shineImage, new Color(1, 1, 1, 0), new Color(1f, 0.7f, 0.7f, 0.6f));

        _bounceAnimation = new BounceAnimation(_costImagePanel);

        _cameraShake = new CameraShake(Camera.main, _marbleAnimator);
    }

    public void OnButtonClick()
    {
        Click?.Invoke();
    }

    public void OnSuccessBoostBuy()
    {
        WaitCustom.Instance.Play(_shineAnimation, _backShineAnimation.Speed);


        _cameraShake.SetRandomPosition();
        WaitCustom.Instance.Play(_cameraShake, _cameraShakeSpeed);

        WaitCustom.Instance.Play(_bounceAnimation, _onSuccessBuy.Speed);
        _bounceAnimation.SetAmplitude(_onSuccessBuy.Amplitude);

        WaitCustom.Instance.Play(_upgradeAnimation, _onSuccessBuy.ShineSpeed);
    }
    
    public void OnFailedBoostBuy()
    {
        WaitCustom.Instance.Play(_bounceAnimation, _onFailedBuy.Speed);
        _bounceAnimation.SetAmplitude(_onFailedBuy.Amplitude);

        WaitCustom.Instance.Play(_blockAnimation, _onFailedBuy.ShineSpeed);
    }
    
    public void SetView(int level, int cost, float boost)
    {
        _boostText.text = "LVL: " + (level + 1);

        _abilityCost.text = NumberParser.FromNumberToShortText(cost);
    }

    public void WhenMoneyChange(int abilityCost)
    {
        _blockPanel.SetActive(Wallet.instance.Value < abilityCost);

        //Color activnessColor = Wallet.instance.Value >= abilityCost ? Color.white : Color.gray;

        //_buttonImage.color = activnessColor;

        //_abilityCost.color = activnessColor;
    }
}

[System.Serializable]
public class ButtonBounceAnimateParametres
{
    public float ShineSpeed;

    public float Speed;

    public float Amplitude;
}