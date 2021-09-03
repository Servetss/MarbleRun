using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour
{
    [SerializeField] private Text _boostText;

    [SerializeField] private Text _abilityCost;

    [Header("Components to color change")]
    [SerializeField] private Image _buttonImage;

    public Action Click;

    public void OnButtonClick()
    {
        Click?.Invoke();
    }

    public void SetView(int level, int cost, float boost)
    {
        _boostText.text = (level * 10) + "%";

        _abilityCost.text = NumberParser.FromNumberToShortText(cost);
    }

    public void WhenMoneyChange(int abilityCost)
    {
        Color activnessColor = Wallet.instance.Value >= abilityCost ? Color.white : Color.gray;

        _buttonImage.color = activnessColor;

        _abilityCost.color = activnessColor;
    }
}
