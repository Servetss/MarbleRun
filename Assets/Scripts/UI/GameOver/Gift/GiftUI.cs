﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class GiftUI : MonoBehaviour, IGameOverPanels
{
    [SerializeField] private GameObject _giftButton;

    [SerializeField] private GameObject _acceptButton;

    [SerializeField] private SkinContainer _skinContainer;

    [SerializeField] private GiftReciveUI _giftReciveUI;

    [Header("Grift")]
    [SerializeField] private GameObject _giftFilling;

    [SerializeField] private GameObject _giftReady;

    [Range(0, 1)]
    [SerializeField] private float _fillLevel;

    public LevelInfo LevelInfo { get; private set; }
    
    private ImageFiller _imageFiller;

    private ButtonActivness _buttonActivness;

    private GameOverPanel _gameOverPanel;

    private Animator _animator;

    public void Awake()
    {
        //PlayerPrefs.DeleteAll();

        _imageFiller = GetComponent<ImageFiller>();

        _animator = GetComponent<Animator>();

        _buttonActivness = new ButtonActivness(_giftButton, this);

        ButtonsAddListeners();

        if (_imageFiller == null || _skinContainer == null)
            throw new ArgumentNullException();

        _imageFiller.FillFinished += FinishFillGiftImage;

        _imageFiller.Load();
    }

    private void ButtonsAddListeners()
    {
        _giftButton.GetComponent<Button>().onClick.AddListener(() => _buttonActivness.OnClick(_imageFiller.IsFill));

        _acceptButton.GetComponent<Button>().onClick.AddListener(_buttonActivness.ResetData);
    }

    public void Init(GameOverPanel gameOverPanel)
    {
        _gameOverPanel = gameOverPanel;
    }

    public void OpenPanel(LevelInfo levelInfo)
    {
        _giftFilling.SetActive(true);

        _giftReady.SetActive(false);
        
        LevelInfo = levelInfo;

        float fillLevel = levelInfo.PlayerLevel <= 2 ? 0.5f : 0.2f;

        StartFillGiftImage(fillLevel); //0.2f
    }

    public void ClosePanel()
    {
        _animator.SetTrigger("Close");

        _imageFiller.ClearData();

        _acceptButton.SetActive(false);

        _giftFilling.SetActive(true);
        
        _gameOverPanel.GoToNextPanel();
    }

    #region Default Activness
    [ContextMenu("Start Fill Gift Image")]
    public void StartFillGiftImage(float fillRange)
    {
        _imageFiller.StartFill(fillRange);

        _buttonActivness.WhenGiftImageFilling(_imageFiller.IsFill);
    }

    private void FinishFillGiftImage(bool isGiftReady)
    {
        _buttonActivness.WhenFinishGiftImageFilling(isGiftReady);

        if (isGiftReady)
        {
            _giftFilling.SetActive(false);

            _giftReady.SetActive(true);
        }

        if (_acceptButton.activeSelf == false)
            Invoke("ShowAcceptButton", 1);
    }

    private void ShowAcceptButton()
    {
        _acceptButton.SetActive(true);
    }
    #endregion

    #region Gift Unlocked
    public void UnlockAFirstSkin()
    {
        CancelInvoke("ShowAcceptButton");

        _animator.SetTrigger("Open");

        _giftButton.SetActive(false);

        _acceptButton.SetActive(false);

        _giftReciveUI.FillGiftAndUnlockSkin(_skinContainer.GetFirstLockedSkinOrNull());
    }

    
    public void UlockASkin()
    {
        CancelInvoke("ShowAcceptButton");

        _animator.SetTrigger("Open");

        _giftButton.SetActive(false);

        _acceptButton.SetActive(false);

        _giftReciveUI.FillGift(_skinContainer.GetFirstLockedSkinOrNull());
    }
    #endregion
}
