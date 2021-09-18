using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private EventMachine _playerEvent;

    [Header("Audio source")]
    [SerializeField] private AudioSource _playerAudioSource;

    [SerializeField] private AudioSource _3DAudioSource;

    [SerializeField] private AudioSource _music;

    [Header("Audio clips")]
    [SerializeField] private AudioClip _startGameAudio;

    [SerializeField] private AudioClip _coinAudio;

    [SerializeField] private AudioClip _obstacleAudio;

    [SerializeField] private AudioClip _menuClickAudio;

    [SerializeField] private AudioClip _giftBoxOpeningAudio;

    [SerializeField] private AudioClip _xZoneJumpAudio;

    [SerializeField] private AudioClip _finishFireworkAudio;

    private int _firewarkCount = 0;

    private bool _isSoundActive;

    private bool _isVibrationActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerEvent?.SubscribeOnMoveToNextLevel(OnMusicVolumeNormal);

        _playerEvent?.SubscribeOnRoadStartStart(OnMusicVolumeQuite);
    }

    private void OnMusicVolumeNormal()
    {
        _music.volume = 0.8f;
    }

    private void OnMusicVolumeQuite()
    {
        _music.volume = 0.25f;
    }

    public void SetSoundActive(bool active)
    {
        _isSoundActive = active;

        _playerAudioSource.mute = !active;

        _3DAudioSource.mute = !active;

        _music.mute = !active;
    }

    public void SetVibrationActive(bool active)
    {
        _isVibrationActive = active;
    }

    public void Vibration()
    {
        if(_isVibrationActive)
            Handheld.Vibrate();
    }

    public void OnGameStart()
    {
        _playerAudioSource.PlayOneShot(_startGameAudio);
    }

    public void OnMenuButtonClick()
    {
        _playerAudioSource.clip = _menuClickAudio;

        _playerAudioSource.Play();
    }

    public void OnCoinPickUp()
    {
        _playerAudioSource.clip = _coinAudio;

        _playerAudioSource.Play();
    }

    public void OnObstacleCrash(Vector3 position)
    {
        _playerAudioSource.PlayOneShot(_obstacleAudio);
    }

    public void OnXZoneJump()
    {
        _playerAudioSource.clip = _xZoneJumpAudio;

        _playerAudioSource.Play();
    }

    public void OnGiftBoxOpening()
    {
        _playerAudioSource.clip = _giftBoxOpeningAudio;

        _playerAudioSource.Play();
    }

    public void FinishFirewark()
    {
        _playerAudioSource.PlayOneShot(_finishFireworkAudio);

        _firewarkCount++;
        if (_firewarkCount >= 3)
            return;

        Invoke("FinishFirewark", 0.8f);
    }
}
