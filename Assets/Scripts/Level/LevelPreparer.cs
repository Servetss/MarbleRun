using Barmetler.RoadSystem;
using System;
using UnityEngine;

public class LevelPreparer : MonoBehaviour
{
    [SerializeField] private LevelContainer _levelContainer;

    [SerializeField] private Player _player;

    [SerializeField] private Finish _finish;

    [SerializeField] private PlayerEventMachine _playerEventMachine;

    private Transform _playerTransform;

    private Transform _finishTransform;

    private int _previousLevelIndex;

    private void Awake()
    {
        if (_levelContainer == null)
            throw new ArgumentNullException();
    }

    public int SelectedLevelIndex { get; private set; }

    public Road SelecetedRoad { get => _levelContainer.GetLevelByIndex(SelectedLevelIndex).GetComponent<Road>(); }

    private void Start()
    {
        _playerTransform = _player.transform;

        _finishTransform = _finish.transform;
    }

    public void SelectNewLevel()
    {
        SelectLevelIndex();

        Replace();

        SetPlayerAndFinishOnTheLevel();

        _playerEventMachine.NextLevelMethod();
    }

    private void SelectLevelIndex()
    {
        _previousLevelIndex = SelectedLevelIndex;

        SelectedLevelIndex++;

        if (SelectedLevelIndex >= _levelContainer.LevelsCount)
        {
            SelectedLevelIndex = 0;
        }
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

    private void SetPlayerAndFinishOnTheLevel()
    {
        Level level = _levelContainer.GetLevelByIndex(SelectedLevelIndex).GetComponent<Level>();

        _playerTransform.position = level.StartTransform.position;

        _playerTransform.rotation = level.StartTransform.rotation;

        _finishTransform.position = level.FinishTransform.position;
    }
}
