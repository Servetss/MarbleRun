using UnityEngine;
using UnityEngine.UI;

public class WalletView : MonoBehaviour
{
    [SerializeField] private Text _moneyText;

    private void Start()
    {
        Wallet.instance.SubscribeOnMoneyChange(MoneyChange);
    }

    private void MoneyChange()
    {
        _moneyText.text = Wallet.instance.Value.ToString();
    }
}
