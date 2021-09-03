using UnityEngine;
using UnityEngine.UI;

public class BoostView : MonoBehaviour
{
    [SerializeField] private GameObject _boostPanel;

    [SerializeField] private EventMachine _playerEventMachine;

    [SerializeField] private RoadMover _playerRoadMover;

    [SerializeField] private Image _boostFill;

    private bool _isBoostZone;

    public float To => (_playerRoadMover.Speed - 60) / 40;

    private void Start()
    {
        _playerEventMachine.SubscribeOnBoostZoneStart(StartBoostZone);

        _playerEventMachine.SubscribeOnBoostZoneFinish(EndBoostZone);

        _playerEventMachine.SubscribeOnRoadEnd(RoadEnd);
    }

    private void FixedUpdate()
    {
        if (_isBoostZone)
        {
            if (_boostFill.fillAmount < To)
                _boostFill.fillAmount += Time.fixedDeltaTime * 0.05f;
            else if (_boostFill.fillAmount > To)
            {
                _boostFill.fillAmount -= Time.fixedDeltaTime * 0.05f;
            }
        }
    }

    public void SetFillAmount(int min, int max, float value)
    {
        value -= min;

        max -= min;

       // _to = value / max;
    }

    private void StartBoostZone()
    {
        _boostPanel.SetActive(true);

        //To = 0;

        _boostFill.fillAmount = 0;

        _isBoostZone = true;
    }

    private void EndBoostZone()
    {
        _isBoostZone = false;
    }

    private void RoadEnd()
    {
        _boostPanel.SetActive(false);
    }
}
