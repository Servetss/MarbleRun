using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;

    [SerializeField] private GameObject _skinPanel;

    [SerializeField] private GameObject _startButton;

    public void OpenMainMenuPanel()
    {
        _mainMenuPanel.SetActive(true);

        _startButton.SetActive(true);

        _skinPanel.SetActive(false);
    }

    public void OpenSkinPanel()
    {
        _mainMenuPanel.SetActive(false);

        _skinPanel.SetActive(true);
    }
}
