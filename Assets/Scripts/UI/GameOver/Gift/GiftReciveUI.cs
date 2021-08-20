using UnityEngine;
using UnityEngine.UI;

public class GiftReciveUI : MonoBehaviour
{
    [SerializeField] private Text _giftName;

    [SerializeField] private GameObject _acceptButton;

    public void FillGift(SkinSO skin)
    {
        _giftName.text = skin.name;

        gameObject.SetActive(true);

        _acceptButton.SetActive(true);
    }

    public void CloseGiftUnlocket()
    {
        gameObject.SetActive(false);

        _acceptButton.SetActive(false);
    }
}
