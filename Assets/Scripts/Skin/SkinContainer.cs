using System;
using UnityEngine;

public class SkinContainer : MonoBehaviour
{
    [SerializeField] private SkinSO[] _skins;

    [SerializeField] private SkinChangerView _skinChangerView;

    private SkinModel _skinModel;

    private SkinPresenter _skinPresenter;

    public Action OnShopBackClick;

    private void Awake()
    {
        SkinLoads();

        _skinModel = new SkinModel(this);

        _skinPresenter = new SkinPresenter(_skinChangerView, _skinModel);

        _skinChangerView.OnBackClick += OnBackShopClick;
    }

    private void SkinLoads()
    {
        for (int i = 0; i < _skins.Length; i++)
        {
            _skins[i].Load();
        }
    }

    private void OnEnable()
    {
        _skinPresenter.Enable();
    }

    private void OnDisable()
    {
        _skinPresenter.Disable();
    }

    public SkinSO GetSkinByIndex(int index)
    {
        return _skins[index];
    }

    public SkinSO GetFirstLockedSkinOrNull()
    {
        for (int i = 0; i < _skins.Length; i++)
        {
            if (_skins[i].IsUnlocked == false)
                return _skins[i];
        }

        return null;
    }

    public void OnBackShopClick()
    {
        _skinModel.SelectPreviousUnlocked();
    }

    public int GetNextSkinIndex(int actualIndex)
    {
        return GetClosestIndex(_skins.Length, actualIndex, 1);
    }

    public int GetPreviousSkinIndex(int actualIndex)
    {
        return GetClosestIndex(_skins.Length, actualIndex, -1);
    }

    public int GetNextUnLockedSkinIndex(int actualIndex)
    {
        actualIndex = GetClosestIndex(_skins.Length, actualIndex, 1);

        for (int i = 0; i < _skins.Length; i++)
        {
            if (_skins[actualIndex].IsUnlocked)
            {
                return actualIndex;
            }

            actualIndex = GetClosestIndex(_skins.Length, actualIndex, 1);
        }
        
        return actualIndex;
    }

    private int GetClosestIndex(int listLenght, int index, int direction)
    {
        index += direction;

        if (index < 0)
        {
            index = listLenght - 1;
        }
        else if (index >= listLenght)
        {
            index = 0;
        }

        return index;
    }
}
