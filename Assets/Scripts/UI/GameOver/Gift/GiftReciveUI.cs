using UnityEngine;
using UnityEngine.UI;

public class GiftReciveUI : MonoBehaviour
{
    [SerializeField] private Image _skinImage;

    [SerializeField] private GameObject _acceptButton;

    [SerializeField] private Sprite _coinSprite;

    [SerializeField] private Text _coinRecieveText;

    [Range(0, 100)]
    [SerializeField] private int _posibilityToSkinDrop;

    public void FillGift(SkinSO skin)
    {
        int random = Random.Range(0, 100);

        skin = _posibilityToSkinDrop >= random ? skin : null;

        if (skin == null)
        {
            _skinImage.sprite = _coinSprite;

            _coinRecieveText.text = NumberParser.FromNumberToShortText(2000);

            Wallet.instance.AddMoney(2000);
        }
        else
        {
            _skinImage.sprite = skin.SkinOnUI;

            skin.UnlockTheSkin();
        }

        gameObject.SetActive(true);

        _acceptButton.SetActive(true);
    }

    public void CloseGiftUnlocket()
    {
        _coinRecieveText.text = "";

        gameObject.SetActive(false);

        _acceptButton.SetActive(false);
    }
}
