using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;

    [SerializeField] private GameObject _skinPanel;

    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _playerAOShadow;

    [Header("Animation")]
    [SerializeField] private UIHideAnimation[] _mainMenuHideAnimations;
    
    public void OpenMainMenuPanel()
    {
        _mainMenuPanel.SetActive(true);

        _startButton.SetActive(true);

        _skinPanel.SetActive(false);

        _playerAOShadow.SetActive(true);
    }

    public void OpenMainMenuPanelWithoutStartButton()
    {
        _skinPanel.SetActive(false);
    }

    public void OnShopButtonClick()
    {
        _mainMenuPanel.SetActive(false);

        _playerAOShadow.SetActive(false);
    }

    public void OpenSkinPanel()
    {
        _mainMenuPanel.SetActive(false);

        _skinPanel.SetActive(true);
    }

    // Button Click //
    public void OnStartRaceClick()
    {
        for (int i = 0; i < _mainMenuHideAnimations.Length; i++)
        {
            _mainMenuHideAnimations[i].StartHide();
        }
    }

    public void OnRaceFinishAndGoToMainMenu()
    {
        for (int i = 0; i < _mainMenuHideAnimations.Length; i++)
        {
            _mainMenuHideAnimations[i].SetDefaultPositionShow();
        }
    }
}
