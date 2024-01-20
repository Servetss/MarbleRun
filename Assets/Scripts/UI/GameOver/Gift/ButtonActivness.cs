using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivness
{
    private const string OpenGift = "OPEN GIFT !";

    private const string BoostName = "Boost";

    private GiftUI _giftUI;

    private GameObject _giftButton;

    private bool _isBoostClicked;

    private bool _isPriseClicked;

    public ButtonActivness(GameObject giftButton, GiftUI giftUI)
    {
        _giftButton = giftButton;

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

            _giftUI.StartFillGiftImage(0.2f); // 0.35

            HideButton();
        }
    }

    public void WhenFinishGiftImageFilling(bool isGiftReady)
    {
        if (isGiftReady) //(_isBoostClicked == false && _isPriseClicked == false) || isGiftReady)
        {
            ShowButton();
        }
    }

    public void WhenGiftImageFilling(bool isGiftReady)
    {
        if (isGiftReady)
        {
            HideButton();
        }
        //else
        //{
        //    ShowButton();
        //}
    }

    public void ResetData()
    {
        _isBoostClicked = false;

        _isPriseClicked = false;
    }

    private void ShowButton()
    {
        _giftButton.SetActive(true);
    }

    private void HideButton()
    {
        _giftButton.SetActive(false);
    }
}
