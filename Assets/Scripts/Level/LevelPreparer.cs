using Barmetler;
using Barmetler.RoadSystem;
using System;
using System.Linq;
using UnityEngine;

public class LevelPreparer : MonoBehaviour
{
    private const string LevelSave = "LevelPreparer";

    private const string PreviousSave = "PreviousLevel";

    private const string SelectedSave = "SelectedPreparer";

    [SerializeField] private LevelContainer _levelContainer;

    [SerializeField] private LevelEventZone _levelEventZone;

    [SerializeField] private GameProgressView _gameProgressView;

    [Header("Marbles")]
    [SerializeField] private Player _player;

    [SerializeField] private Enemys _enemys;

    private EventMachine _playerEventMachine;

    private Transform _playerTransform;

    private int _previousLevelIndex;

    private void Awake()
    {
        if (_levelContainer == null)
            throw new ArgumentNullException();

        _playerEventMachine = _player.PlayerEventMachine;
    }

    public Player PlayerMarble { get => _player; }

    public Enemys Enemys { get => _enemys; }

    public int SelectedLevelIndex { get; private set; }

    public int LevelCount { get; private set; }

    public Road SelecetedTrack { get => _levelContainer.GetLevelByIndex(SelectedLevelIndex).GetComponent<Road>(); }

    public LevelEventZone LevelEventZone { get => _levelEventZone; }

    private void Start()
    {
        _playerTransform = _player.transform;

        Load();

        PutOnTheLevel();
    }

    public void SelectNewLevel()
    {
        if (PlayerMarble.LevelInfo.IsWin)
        {
            SelectLevelIndex();

            _gameProgressView.CompleteLevel();
        }

        PutOnTheLevel();
    }

    private void PutOnTheLevel()
    {
        _playerEventMachine.NextLevelMethod();

        _enemys.NextLevel();

        Replace();

        _levelEventZone.SetEventsToTrack(SelecetedTrack);

        SetPlayerOnTheLevel();
    }

    private void SelectLevelIndex()
    {
        _previousLevelIndex = SelectedLevelIndex;

        SelectedLevelIndex++;

        LevelCount++;

        if (SelectedLevelIndex >= _levelContainer.LevelsCount)
        {
            SelectedLevelIndex = 0;
        }

        Save();
    }

    private void Replace()
    {
        ShowLevel(_levelContainer.GetLevelByIndex(SelectedLevelIndex));

        HideLevel(_levelContainer.GetLevelByIndex(_previousLevelIndex));
    }

    private void ShowLevel(GameObject level)
    {
        level.SetActive(true);
    }

    private void HideLevel(GameObject level)
    {
        level.SetActive(false);
    }

    private void SetPlayerOnTheLevel()
    {
        _levelEventZone.SetPlayersOnStartZone(_enemys.GetEnemiesTransform(), _playerTransform);
    }

    #region Save\load
    private void Save()
    {
        PlayerPrefs.SetInt(LevelSave + PreviousSave, _previousLevelIndex);

        PlayerPrefs.SetInt(LevelSave + SelectedSave, SelectedLevelIndex);
    }

    private void Load()
    {
        _previousLevelIndex = PlayerPrefs.GetInt(LevelSave + PreviousSave);

        SelectedLevelIndex = PlayerPrefs.GetInt(LevelSave + SelectedSave);
    }
    #endregion
}
