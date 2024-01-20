using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinChangerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private MeshRenderer _sphere;

    [SerializeField] private GameObject _selected_B;

    [SerializeField] private GameObject _buy_B;

    [SerializeField] private TextMeshProUGUI _buyText;

    [SerializeField] private TextMeshProUGUI _skinCost_T;
    
    private MainMenuPanel _mainMenuPanel;

    public Action ClickOnNextSkin;

    public Action ClickOnPreviousSkin;

    public Action ClickOnBuySkin;

    public Action ClickOnSelectedSkin;

    public Action OnBackClick;

    private void Awake()
    {
        _mainMenuPanel = GetComponent<MainMenuPanel>();
    }

    public void OnBuySkin()
    {
        ClickOnBuySkin?.Invoke();
    }

    public void OnSelectSkin()
    {
        ClickOnSelectedSkin?.Invoke();
    }

    public void OnSkinModeEnable()
    {
        _mainMenuPanel.OnShopButtonClick();

        _animator.SetBool("ChangeSkin", true);
    }

    public void OnSkinModeDisable()
    {
        OnBackClick?.Invoke();
        
        _mainMenuPanel.OpenMainMenuPanelWithoutStartButton();

        _animator.SetBool("ChangeSkin", false);
    }

    public void SetSkin(SkinSO skin)
    {
        if (skin == null)
            return;

        _sphere.material = skin.Material;

        if (skin.IsUnlocked)
        {
            EnableSelectedButton();
        }
        else
        {
            EnableBuyButton();

            _skinCost_T.text = NumberParser.FromNumberToShortText(skin.SkinCost);

            Color activnessColor = Wallet.instance.Value >= skin.SkinCost ? Color.white : Color.gray;

            _buy_B.GetComponent<Image>().color = activnessColor;

            _skinCost_T.color = activnessColor;

            _buyText.color = activnessColor;
        }
    }

    public void OnMoveToNextSkinClick()
    {
        ClickOnNextSkin?.Invoke();
    }

    public void OnMoveToPreviousSkinClick()
    {
        ClickOnPreviousSkin?.Invoke();
    }

    private void EnableBuyButton()
    {
        _selected_B.SetActive(false);

        _buy_B.SetActive(true);
    }

    private void EnableSelectedButton()
    {
        _selected_B.SetActive(true);

        _buy_B.SetActive(false);
    }
}
