using System;
using UnityEngine;

public class LevelPreparer : MonoBehaviour
{
    [SerializeField] private LevelContainer _levelContainer;

    [SerializeField] private Player _player;

    [SerializeField] private Finish _finish;

    private Transform _playerTransform;

    private Transform _finishTransform;

    private int _selectedLevelIndex;

    private int _previousLevelIndex;

    private void Awake()
    {
        if (_levelContainer == null)
            throw new ArgumentNullException();
    }

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

        _player.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SelectLevelIndex()
    {
        _previousLevelIndex = _selectedLevelIndex;

        _selectedLevelIndex++;

        if (_selectedLevelIndex >= _levelContainer.LevelsCount)
        {
            _selectedLevelIndex = 0;
        }
    }

    private void Replace()
    {
        ShowLevel(_levelContainer.GetLevelByIndex(_selectedLevelIndex));

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
        Level level = _levelContainer.GetLevelByIndex(_selectedLevelIndex).GetComponent<Level>();

        _playerTransform.position = level.StartPosition;

        _finishTransform.position = level.FinishPosition;
    }
}
