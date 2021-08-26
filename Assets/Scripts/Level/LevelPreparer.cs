﻿using Barmetler.RoadSystem;
using System;
using UnityEngine;

public class LevelPreparer : MonoBehaviour
{
    private const string LevelSave = "LevelPreparer";
    
    private const string PreviousSave = "PreviousLevel";

    private const string SelectedSave = "SelectedPreparer";

    [SerializeField] private LevelContainer _levelContainer;

    [SerializeField] private Player _player;

    [SerializeField] private Finish _finish;

    private EventMachine _playerEventMachine;

    private Transform _playerTransform;

    private Transform _finishTransform;

    private int _previousLevelIndex;

    private void Awake()
    {
        if (_levelContainer == null)
            throw new ArgumentNullException();

        _playerEventMachine = _player.PlayerEventMachine;
    }

    public int SelectedLevelIndex { get; private set; }

    public Road SelecetedRoad { get => _levelContainer.GetLevelByIndex(SelectedLevelIndex).GetComponent<Road>(); }

    private void Start()
    {
        _playerTransform = _player.transform;

        _finishTransform = _finish.transform;

        Load();

        PutOnTheLevel();
    }

    public void SelectNewLevel()
    {
        SelectLevelIndex();

        PutOnTheLevel();
    }

    private void PutOnTheLevel()
    {
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

    private void SetPlayerAndFinishOnTheLevel()
    {
        Level level = _levelContainer.GetLevelByIndex(SelectedLevelIndex).GetComponent<Level>();

        _playerTransform.position = level.StartTransform.position;

        _playerTransform.rotation = level.StartTransform.rotation;

        _playerTransform.GetChild(0).localPosition = Vector3.zero;

        _playerTransform.GetChild(0).localEulerAngles = Vector3.zero;

        _finishTransform.position = level.FinishTransform.position;
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
