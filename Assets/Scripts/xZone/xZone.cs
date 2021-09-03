using UnityEngine;
using UnityEngine.UI;

public class xZone : MonoBehaviour
{
    [SerializeField] private int _xBoost;

    [SerializeField] private Text _boostText;

    private void Start()
    {
        _boostText.text = "X" + _xBoost.ToString();
    }

    public int Boost { get => _xBoost; }
}
