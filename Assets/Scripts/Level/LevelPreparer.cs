using Barmetler.RoadSystem;
using System;
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

    [Header("Background")]
    [SerializeField] private Transform _water;

    [SerializeField] private Transform _background;

    private EventMachine _playerEventMachine;

    private Transform _playerTransform;

    private int _previousLevelIndex;

    private void Awake()
    {
        if (_levelContainer == null)
            throw new ArgumentNullException();

        _playerEventMachine = _player.GetComponent<EventMachine>();

        //ResetLoad();
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

        HideNotActiveLevels();

        _gameProgressView.ShowLevel();

        Invoke("PutOnTheLevel", Time.fixedDeltaTime);
    }

    private void HideNotActiveLevels()
    {
        for (int i = 0; i < _levelContainer.LevelsCount; i++)
        {
            if(i != SelectedLevelIndex)
                HideLevel(_levelContainer.GetLevelByIndex(i));
        }
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
        HideLevel(_levelContainer.GetLevelByIndex(_previousLevelIndex));

        ShowLevel(_levelContainer.GetLevelByIndex(SelectedLevelIndex));

        Vector3 levelPosition = _levelContainer.GetLevelByIndex(SelectedLevelIndex).transform.position;

        _water.position = new Vector3(levelPosition.x, -117.3f, levelPosition.z);

        _background.position = new Vector3(levelPosition.x, -117.3f, levelPosition.z);
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

        PlayerPrefs.SetInt(LevelSave + "LevelCount", LevelCount);
    }

    private void Load()
    {
        _previousLevelIndex = PlayerPrefs.GetInt(LevelSave + PreviousSave);

        SelectedLevelIndex = PlayerPrefs.GetInt(LevelSave + SelectedSave);

        LevelCount = PlayerPrefs.GetInt(LevelSave + "LevelCount");
    }

    private void ResetLoad()
    {
        PlayerPrefs.SetInt(LevelSave + PreviousSave, 0);

        PlayerPrefs.SetInt(LevelSave + SelectedSave, 0);

        PlayerPrefs.SetInt(LevelSave + "LevelCount", 0);
    }
    #endregion
}
