using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    [SerializeField] private MeshRenderer _playerSphere;

    private SkinContainer _skinContainer;

    private int _skinNum;

    private void Awake()
    {
        _skinContainer = GetComponent<SkinContainer>();
    }

    public void OnButtonClick()
    {
        _skinNum++;

        _skinNum = _skinContainer.GetNextUnLockedSkinIndex(_skinNum);

        SkinSO skin = _skinContainer.GetSkinByIndex(_skinNum);

        SetSkin(skin);
    }

    public void SetSkin(SkinSO skin)
    {
        Debug.Log(skin.Name);

        _playerSphere.material = skin.Material;
    }
}
