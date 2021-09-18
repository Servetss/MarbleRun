using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private EventMachine _eventMachine;

    [SerializeField] private GameObject[] _backgroundList;

    [SerializeField] private int _previousBack;

    [SerializeField] private int _actualBack;

    private void Start()
    {
        _eventMachine?.SubscribeOnMoveToNextLevel(SetNextBackground);
    }

    private void SetNextBackground()
    {
        //Replace();

       _previousBack = _actualBack;

        _actualBack++;

        if (_actualBack >= _backgroundList.Length)
            _actualBack = 0;
    }

    private void Replace()
    {
        _backgroundList[_previousBack].SetActive(false);

        _backgroundList[_actualBack].SetActive(true);
    }
}
