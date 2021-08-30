using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour
{
    [SerializeField] private Text _boostText;

    [SerializeField] private Text _abilityCost;

    public Action Click;

    public void OnButtonClick()
    {
        Click?.Invoke();
    }

    public void SetView(int level, int cost, int boost)
    {
        _boostText.text = (level * 10) + "%";

        _abilityCost.text = cost.ToString();
    }
}
