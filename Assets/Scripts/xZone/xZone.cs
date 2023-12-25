using UnityEngine;
using UnityEngine.UI;

public class xZone : MonoBehaviour
{
    [SerializeField] private int _xBoost;

    [SerializeField] private Text _boostText;
    
    private XZoneAnimate xZoneTriggerAnimation;

    public int Boost => _xBoost;

    private void Awake()
    {
        xZoneTriggerAnimation = new XZoneAnimate(transform, 1.5f);
    }

    private void Start()
    {
        _boostText.text = "X" + _xBoost.ToString();
    }

    public void OnCustomTrigger(float impulse)
    {
        if (impulse < 5) return;

        xZoneTriggerAnimation.ChangeHeight(impulse);

        WaitCustom.Instance.Play(xZoneTriggerAnimation, 0.15f);
    }
}
