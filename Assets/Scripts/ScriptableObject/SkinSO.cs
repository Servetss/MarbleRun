using UnityEngine;

[CreateAssetMenu(fileName = "Skin SO", menuName = "ScriptableObjects/Skin", order = 1)]
public class SkinSO : ScriptableObject
{
    private const string SaveSkin = "Skin";

    [SerializeField] private string _skinName;

    [SerializeField] private int _skinCost;

    [SerializeField] private Sprite _skinOnUI;

    [SerializeField] private Material _material;

    [SerializeField] private bool _isUnlocked;

    [SerializeField] private bool _isDefault;

    public string Name { get => _skinName; }

    public int SkinCost { get => _skinCost; }

    public Sprite SkinOnUI { get => _skinOnUI; }

    public Material Material { get => _material; }

    public bool IsUnlocked { get => _isUnlocked || _skinCost == 0; }

    public void UnlockTheSkin()
    {
        _isUnlocked = true;

        Save();
    }

    #region Save\Load
    public void Save()
    {
        PlayerPrefs.SetInt(SaveSkin + name, _isUnlocked ? 1 : 0);
    }

    public void Load()
    {
        _isUnlocked = PlayerPrefs.GetInt(SaveSkin + name) == 1;
    }
    #endregion
}
