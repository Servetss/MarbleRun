using UnityEngine;

public class SkinContainer : MonoBehaviour
{
    [SerializeField] private SkinSO[] _skins;

    [SerializeField] private SkinChangerView _skinChangerView;

    private SkinPresenter _skinPresenter;

    private void Awake()
    {
        SkinModel skinModel = new SkinModel(this);

        _skinPresenter = new SkinPresenter(_skinChangerView, skinModel);
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
