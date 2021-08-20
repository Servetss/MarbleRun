using UnityEngine;

[CreateAssetMenu(fileName = "Skin SO", menuName = "ScriptableObjects/Skin", order = 1)]
public class SkinSO : ScriptableObject
{
    [SerializeField] private string _skinName;

    [SerializeField] private Material _material;

    [SerializeField] private bool _isUnlocked;

    public string Name { get => _skinName; }

    public Material Material { get => _material; }

    public bool IsUnlocked { get => _isUnlocked; }
}
