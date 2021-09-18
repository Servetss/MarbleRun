using UnityEngine;

public class SettingsModel : MonoBehaviour
{
    private const string SettingSave = "Setting";

    private const string VibrationSave = "Vibration";

    private const string MusicSave = "Music";

    [SerializeField] private SettingView _settingView;

    private void Awake()
    {
        Load();

        _settingView.RefreshUI();
    }

    public bool IsVibrationOn { get; private set; }

    public bool IsMusicOn { get; private set; }

    public void OnVibrationClick()
    {
        IsVibrationOn = !IsVibrationOn;

        SoundManager.Instance.SetVibrationActive(IsVibrationOn);

        Save();
    }

    public void OnMusicClick()
    {
        IsMusicOn = !IsMusicOn;

        SoundManager.Instance.SetSoundActive(IsMusicOn);

        Save();
    }

    public void OnRateClick()
    {
        
    }

    #region Save \ Load
    public void Save()
    {
        PlayerPrefs.SetInt(SettingSave + VibrationSave, IsVibrationOn ? 1 : 0);

        PlayerPrefs.SetInt(SettingSave + MusicSave, IsMusicOn ? 1 : 0);
    }

    public void Load()
    {
        bool isFirstGameOpen = PlayerPrefs.HasKey(SettingSave + VibrationSave) == false;

        if (isFirstGameOpen == false)
        {
            IsVibrationOn = PlayerPrefs.GetInt(SettingSave + VibrationSave) == 1;

            IsMusicOn = PlayerPrefs.GetInt(SettingSave + MusicSave) == 1;

            SoundManager.Instance.SetSoundActive(IsMusicOn);

            SoundManager.Instance.SetVibrationActive(IsVibrationOn);
        }
        else
        {
            SoundManager.Instance.SetSoundActive(true);

            SoundManager.Instance.SetVibrationActive(true);
        }

    }
    #endregion
}
