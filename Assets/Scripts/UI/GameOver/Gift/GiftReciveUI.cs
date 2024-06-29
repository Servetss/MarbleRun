using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftReciveUI : MonoBehaviour
{
    [SerializeField] private Image _skinImage;

    [SerializeField] private GameObject _acceptButton;

    [SerializeField] private Sprite _coinSprite;

    [SerializeField] private TextMeshProUGUI _coinRecieveText;

    [Range(0, 100)]
    [SerializeField] private int _posibilityToSkinDrop;

    private int[] _coinRecieves = new int[] { 150, 200, 250, 300 };

    public void FillGiftAndUnlockSkin(SkinSO skin)
    {
        _skinImage.sprite = skin.SkinOnUI;

        skin.UnlockTheSkin();

        gameObject.SetActive(true);

        _acceptButton.SetActive(true);
    }
    
    public void FillGift(SkinSO skin)
    {
        int random = Random.Range(0, 100);

        skin = _posibilityToSkinDrop >= random ? skin : null;

        if (skin == null || skin.SkinOnUI == null)
        {
            _skinImage.sprite = _coinSprite;


            int randomCoin = Random.Range(0, _coinRecieves.Length);
            int coinRecieve = _coinRecieves[randomCoin];

            _coinRecieveText.text = NumberParser.FromNumberToShortText(coinRecieve) + "$";

            Wallet.instance.AddMoney(coinRecieve);
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
