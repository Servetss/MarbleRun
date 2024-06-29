using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private RectTransform _handTransform;

    private AnimatedData _sineAnimation;

    private bool _isStart;
    
    private void Start()
    {
        if (PlayerPrefs.GetInt("HandTutorial") == 0)
        {
            ShowTutorial();

            _sineAnimation = new SineAnimate(_handTransform);

            WaitCustom.Instance.Play(_sineAnimation, 0.4f, true);
        }
    }

    private void ShowTutorial()
    {
        _isStart = true;


        _handTransform.gameObject.SetActive(true);
    }

    public void HideTutorial()
    {
        if (_isStart == false) return;

        int animationIndex = WaitCustom.Instance.IsAnimationPlayed(_sineAnimation);

        if (animationIndex >= 0) WaitCustom.Instance.OnFinish(animationIndex);

        _isStart = false;

        _handTransform.gameObject.SetActive(false);

        PlayerPrefs.SetInt("HandTutorial", 1);
    }
}
