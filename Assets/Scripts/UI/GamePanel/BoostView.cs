using UnityEngine;
using UnityEngine.UI;

public class BoostView : MonoBehaviour
{
    [SerializeField] private GameObject _boostPanel;

    [SerializeField] private EventMachine _playerEventMachine;

    [SerializeField] private RoadMover _playerRoadMover;

    [SerializeField] private Image _boostFill;

    private Accelerator _accelerator;

    private bool _isBoostZone;

    public float To => (_playerRoadMover.Speed - 60) / 40;

    private float _speedView;
    
    private void Start()
    {
        _playerEventMachine.SubscribeOnBoostZoneStart(StartBoostZone);

        _playerEventMachine.SubscribeOnBoostZoneFinish(EndBoostZone);

        _playerEventMachine.SubscribeOnRoadEnd(RoadEnd);

        _accelerator = _playerRoadMover.GetComponent<Accelerator>();
    }

    private void Update()
    {
        if (_isBoostZone)
        {
            float maxSpeed = _accelerator.BoostMaximalSpeed - _accelerator.MaximalSpeed;

            float speed = Mathf.Abs(_playerRoadMover.Speed - _accelerator.MaximalSpeed);

            float fillAmount = speed / maxSpeed;

            _speedView = Mathf.Lerp(_boostFill.fillAmount, fillAmount, 0.1f);

            _boostFill.fillAmount = _speedView;
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
