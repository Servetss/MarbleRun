using UnityEngine;

[CreateAssetMenu(fileName = "Skin SO", menuName = "ScriptableObjects/Skin", order = 1)]
public class SkinSO : ScriptableObject
{
    private const string SaveSkin = "Skin";

    [SerializeField] private string _skinName;

    [SerializeField] private Material _material;

    [SerializeField] private bool _isUnlocked;

    [SerializeField] private bool _isDefault;

    public string Name { get => _skinName; }

    public Material Material { get => _material; }

    public bool IsUnlocked { get => _isUnlocked; }

    public void UnlockTheSkin()
    {
        _isUnlocked = true;

        Save();
    }

    #region Save\Load
    public void Save()
    {
        PlayerPrefs.SetInt(SaveSkin + _skinName, _isUnlocked ? 1 : 0);
    }

    public void Load()
    {
        _isUnlocked = PlayerPrefs.GetInt(SaveSkin + _skinName) == 1;
    }
    #endregion
}
