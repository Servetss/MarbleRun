using System;
using UnityEngine;
using UnityEngine.UI;

public class GiftUI : MonoBehaviour, IGameOverPanels
{
    [SerializeField] private GameObject _giftButton;

    [SerializeField] private GameObject _acceptButton;

    [SerializeField] private SkinContainer _skinContainer;

    [SerializeField] private GiftReciveUI _giftReciveUI;

    private ImageFiller _imageFiller;

    private ButtonActivness _buttonActivness;

    private GameOverPanel _gameOverPanel;

    public void Awake()
    {
        _imageFiller = GetComponent<ImageFiller>();

        _buttonActivness = new ButtonActivness(_giftButton.transform.GetChild(0).GetComponent<Text>(), this);

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
        StartFillGiftImage();
    }

    public void ClosePanel()
    {
        _imageFiller.ClearData();

        _acceptButton.SetActive(false);

        _gameOverPanel.GoToNextPanel();
    }

    #region Default Activness
    [ContextMenu("Start Fill Gift Image")]
    public void StartFillGiftImage()
    {
        _imageFiller.StartFill(0.2f);

        _buttonActivness.WhenGiftImageFilling(_imageFiller.IsFill);
    }

    private void FinishFillGiftImage(bool isGiftReady)
    {
        _buttonActivness.WhenFinishGiftImageFilling(isGiftReady);



        if (_acceptButton.activeSelf == false)
            Invoke("ShowAcceptButton", 1);
    }

    private void ShowAcceptButton()
    {
        _acceptButton.SetActive(true);
    }
    #endregion

    #region Gift Unlocked

    public void UlockASkin()
    {
        CancelInvoke("ShowAcceptButton");

        _giftButton.SetActive(false);

        _acceptButton.SetActive(false);

        _giftReciveUI.FillGift(_skinContainer.GetRandomSkin());
    }

    #endregion
}
