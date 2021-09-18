using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    [SerializeField] private SettingsModel _settingModel;

    [Header("Buttons sprites")]
    [SerializeField] private Sprite _vibrationOn;
    [SerializeField] private Sprite _vibrationOff;

    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;

    [Header("Buttons images")]
    [SerializeField] private Image _vibrationImage;

    [SerializeField] private Image _musicImage;

    public void OnVibrationClick()
    {
        _vibrationImage.sprite = _settingModel.IsVibrationOn ? _vibrationOff : _vibrationOn;

        _settingModel.OnVibrationClick();
    }

    public void OnMusicClick()
    {
        _musicImage.sprite = _settingModel.IsMusicOn ? _musicOff : _musicOn;

        _settingModel.OnMusicClick();
    }

    public void OnRateClick()
    {
        _settingModel.OnRateClick();
    }

    public void RefreshUI()
    {
        _vibrationImage.sprite = _settingModel.IsVibrationOn ? _vibrationOn : _vibrationOff;

        _musicImage.sprite = _settingModel.IsMusicOn ? _musicOn : _musicOff;
    }
}
