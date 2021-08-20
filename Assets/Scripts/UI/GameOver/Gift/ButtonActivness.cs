using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivness
{
    private const string OpenGift = "OPEN GIFT !";

    private const string BoostName = "Boost";

    private GiftUI _giftUI;

    private Text _buttonText;

    private bool _isBoostClicked;

    private bool _isPriseClicked;

    public ButtonActivness(Text buttonText, GiftUI giftUI)
    {
        if (buttonText == null)
            throw new ArgumentNullException();

        _buttonText = buttonText;

        _giftUI = giftUI;
    }

    public void OnClick(bool isGiftReady)
    {
        if (isGiftReady)
        {
            _isPriseClicked = true;

            _giftUI.UlockASkin();
        }
        else
        {
            _isBoostClicked = true;

            _giftUI.StartFillGiftImage();

            HideButton();
        }
    }

    public void WhenFinishGiftImageFilling(bool isGiftReady)
    {
        if((_isBoostClicked == false && _isPriseClicked == false) || isGiftReady)
            ShowButton();
    }

    public void WhenGiftImageFilling(bool isGiftReady)
    {
        if (isGiftReady)
        {
            HideButton();

            _buttonText.text = OpenGift;
        }
        else
        {
            ShowButton();

            _buttonText.text = BoostName;
        }
    }

    public void ResetData()
    {
        _isBoostClicked = false;

        _isPriseClicked = false;
    }

    private void ShowButton()
    {
        _buttonText.transform.parent.gameObject.SetActive(true);
    }

    private void HideButton()
    {
        _buttonText.transform.parent.gameObject.SetActive(false);
    }
}
