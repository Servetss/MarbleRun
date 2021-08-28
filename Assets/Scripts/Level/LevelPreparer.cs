﻿using Barmetler;
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
        SelectLevelIndex();

        PutOnTheLevel();
    }

    private void PutOnTheLevel()
    {
        Replace();

        SetPlayerOnTheLevel();

        _playerEventMachine.NextLevelMethod();

        _enemys.NextLevel();

        _levelEventZone.SetEventsToTrack(SelecetedTrack);
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

    private void SetPlayerOnTheLevel()
    {
        Level level = _levelContainer.GetLevelByIndex(SelectedLevelIndex).GetComponent<Level>();

        Bezier.OrientedPoint[] spline = SelecetedTrack.GetEvenlySpacedPoints(1, 1).Select(e => e.ToWorldSpace(SelecetedTrack.transform)).ToArray();

        _playerTransform.position = spline[0].position;

        _playerTransform.rotation = level.FinishTransform.rotation;

        _playerTransform.GetChild(0).localPosition = Vector3.zero;

        _playerTransform.GetChild(0).localEulerAngles = Vector3.zero;

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
