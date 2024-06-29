using TMPro;
using UnityEngine;

public class xZone : MonoBehaviour
{
    [SerializeField] private float _xBoost;

    [SerializeField] private TextMeshProUGUI _boostText;

    [SerializeField] private MeshRenderer _meshRenderer;
    
    private XZoneAnimate xZoneTriggerAnimation;

    public float Boost => _xBoost;

    private void Awake()
    {
        xZoneTriggerAnimation = new XZoneAnimate(transform, 1.5f);
    }

    public void SetBoost(float boost)
    {
        _xBoost = boost;

        _boostText.text = "X" + _xBoost.ToString();
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }


    public void OnCustomTrigger(float impulse)
    {
        if (impulse < 5) return;

        xZoneTriggerAnimation.ChangeHeight(impulse);

        WaitCustom.Instance.Play(xZoneTriggerAnimation, 0.15f);
    }
}
