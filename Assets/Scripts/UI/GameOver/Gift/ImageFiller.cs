using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageFiller : MonoBehaviour
{
    private const string FillerSave = "FillerSave";

    private const float LerpSpeed = 0.1f;

    [SerializeField] private Image _giftImage;

    private float _fillLevel;

    private float _nextFillLevel;

    private bool _isFilling;

    private float _lerp;

    public Action<bool> FillFinished;

    public bool IsFill { get; private set; }

    public void FixedUpdate()
    {
        if (_isFilling)
        {
            _lerp += Time.fixedDeltaTime * LerpSpeed;

            _giftImage.fillAmount = Mathf.Lerp(_fillLevel, _nextFillLevel, _lerp);

            if (_lerp >= 1)
            {
                _isFilling = false;

                _lerp = 0;

                FillFinished?.Invoke(_giftImage.fillAmount >= 1);
            }
        }
    }

    public void StartFill(float fillRange)
    {
        _fillLevel = _giftImage.fillAmount;

        _nextFillLevel += fillRange;

        IsFill = _nextFillLevel >= 1;

        if (fillRange <= 0)
            throw new InvalidOperationException();

        _isFilling = true;
    }

    public void ClearData()
    {
        if (IsFill)
        {
            _giftImage.fillAmount = 0;

            _nextFillLevel = 0;
        }

        Save();
    }

    #region Save\Load
    private void Save()
    {
        PlayerPrefs.SetFloat(FillerSave, _nextFillLevel);
    }

    public void Load()
    {
        _nextFillLevel = PlayerPrefs.GetFloat(FillerSave);

        _giftImage.fillAmount = _nextFillLevel;
    }
    #endregion
}
