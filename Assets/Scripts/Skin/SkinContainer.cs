using UnityEngine;

public class SkinContainer : MonoBehaviour
{
    [SerializeField] private SkinSO[] _skins;

    public SkinSO GetRandomSkin()
    {
        return _skins[Random.RandomRange(0, _skins.Length)];
    }

    public SkinSO GetSkinByIndex(int index)
    {
        return _skins[index];
    }

    public int GetNextUnLockedSkinIndex(int actualIndex)
    {
        actualIndex = GetNextIndexInList(_skins.Length, actualIndex);

        for (int i = 0; i < _skins.Length; i++)
        {
            if (_skins[actualIndex].IsUnlocked)
            {
                return actualIndex;
            }

            actualIndex = GetNextIndexInList(_skins.Length, actualIndex);
        }
        
        return actualIndex;
    }

    private int GetNextIndexInList(int listLenght, int index)
    {
        index++;

        if (index >= listLenght)
        {
            index = 0;
        }

        return index;
    }
}
