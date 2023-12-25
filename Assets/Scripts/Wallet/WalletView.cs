using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start()
    {
        Wallet.instance.SubscribeOnMoneyChange(MoneyChange);
    }

    private void MoneyChange()
    {
        _moneyText.text = NumberParser.FromNumberToShortText(Wallet.instance.Value);
    }
}
