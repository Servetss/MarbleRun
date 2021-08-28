using System;
using UnityEngine;

public class SkinChangerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private MeshRenderer _sphere;

    public Action ClickOnNextSkin;

    public Action ClickOnPreviousSkin;

    public void ChangeSkinModeOn()
    {
        _animator.SetBool("ChangeSkin", true);
    }

    public void ChangeSkinModeOff()
    {
        _animator.SetBool("ChangeSkin", false);
    }

    public void SetSkin(SkinSO skin)
    {
        _sphere.material = skin.Material;
    }

    public void OnMoveToNextSkinClick()
    {
        ClickOnNextSkin?.Invoke();
    }

    public void OnMoveToPreviousSkinClick()
    {
        ClickOnPreviousSkin?.Invoke();
    }
}
