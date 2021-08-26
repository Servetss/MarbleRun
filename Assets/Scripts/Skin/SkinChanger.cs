using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    private const string SaveSkin = "SkinChanger";

    [SerializeField] private MeshRenderer _playerSphere;

    private SkinContainer _skinContainer;

    private int _skinNum;

    private void Awake()
    {
        _skinContainer = GetComponent<SkinContainer>();
    }

    private void Start()
    {
        Load();
    }

    public void OnButtonClick()
    {
        _skinNum++;

        _skinNum = _skinContainer.GetNextUnLockedSkinIndex(_skinNum);

        SkinSO skin = _skinContainer.GetSkinByIndex(_skinNum);

        SetSkin(skin);

        Save();
    }

    public void SetSkin(SkinSO skin)
    {
        Debug.Log(skin.Name);

        _playerSphere.material = skin.Material;
    }

    #region Save\Load
    public void Save()
    {
        PlayerPrefs.SetInt(SaveSkin, _skinNum);
    }

    public void Load()
    {
        _skinNum = PlayerPrefs.GetInt(SaveSkin);
    }
    #endregion
}

