using AppodealAds.Unity.Android;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitCustom : MonoBehaviour
{
    public static WaitCustom Instance;
    
    [SerializeField] private List<AnimationStruct> _animationStructList;

    private AnimationStruct _actualStructInLoop;
    
    private void Awake()
    {
        Instance = this;

        _animationStructList = new List<AnimationStruct>();
    }

    private void Update()
    {
        for (int i = 0; i < _animationStructList.Count; i++)
        {
            _actualStructInLoop = _animationStructList[i];

            _actualStructInLoop.Time += Time.deltaTime * _actualStructInLoop.Speed;

            _animationStructList[i].AnimatedData.Evaluate(_actualStructInLoop.Time);

            if (_actualStructInLoop.Time >= 1 && _actualStructInLoop.IsLoop == false)
            {
                OnFinish(i);
                
                i--;
            }
        }
    }

    public void OnFinish(int animationDataIndex)
    {
        _animationStructList.RemoveAt(animationDataIndex);
    }
    
    public WaitCustom Play(AnimatedData animData, float speed, bool isLoop = false)
    {
        if (animData == null) return this;

        int itemIndex = IsAnimationPlayed(animData);

        if (itemIndex == -1)
        {
            AnimationStruct animationStruct = new AnimationStruct(animData, speed, isLoop);

            _animationStructList.Add(animationStruct);
        }
        else
        {
            _animationStructList[itemIndex].AnimatedData = animData;

            _animationStructList[itemIndex].Time = 0;

            _animationStructList[itemIndex].Speed = speed;
        }

        return this;
    }

    public int IsAnimationPlayed(AnimatedData animatedData)
    {
        for (int i = 0; i < _animationStructList.Count; i++)
        {
            if (_animationStructList[i].AnimatedData.Equals(animatedData))
            {
                return i;
            }
        }

        return -1;
    }
}

[System.Serializable]
public class AnimationStruct
{
    public AnimatedData AnimatedData;

    public float Time;

    public float Speed;

    public bool IsLoop;

    public AnimationStruct(AnimatedData animatedData, float speed, bool isLoop)
    {
        AnimatedData = animatedData;

        Speed = speed;

        IsLoop = isLoop;
    }
}

public class RectTransformAnimation : AnimatedData
{
    private RectTransform _rectTransform;

    private Vector3 _startPosition;

    private Vector3 _targetPosition;

    public RectTransformAnimation(RectTransform rectTransform, Vector3 targetPosition)
    {
        _rectTransform = rectTransform;

        _startPosition = _rectTransform.anchoredPosition;

        _targetPosition = targetPosition;
    }
    public override void Evaluate(float time)
    {
        _rectTransform.anchoredPosition = Vector3.Lerp(_startPosition, _targetPosition, time);
    }
}

public class XZoneAnimate : AnimatedData
{
    private Transform _xZonePlatform;

    private float _defaultHeight;

    private float _height = 1.5f;
    
    public XZoneAnimate(Transform xZonePlatform, float maxHeight)
    {
        _xZonePlatform = xZonePlatform;

        _defaultHeight = _xZonePlatform.localPosition.z;
        
        _height = maxHeight;
    }

    public void ChangeHeight(float height)
    {
        _height = height * 0.15f;
    }
    
    private float BounceEasing(float time)
    {
        float j = 1;

        float f = 2;
        
        float frequence = Mathf.Abs(Mathf.Sin(time * 2 * Mathf.PI * j));

        float amplitude = Mathf.Pow(1 - time, f);

        return amplitude * frequence;
    }

    public override void Evaluate(float time)
    {
        float animationValue = BounceEasing(time) * _height;
        
        _xZonePlatform.localPosition = new Vector3(_xZonePlatform.localPosition.x, _xZonePlatform.localPosition.y, _defaultHeight - animationValue);
    }
}

public class BounceAnimation : AnimatedData
{
    private Transform _transform;

    private Vector3 _vectorOne;

    private Vector3 _amplitudeVector;

    private float _amplitude;

    public BounceAnimation(Transform transform)
    {
        _transform = transform;

        _vectorOne = Vector3.one;
    }

    public void SetAmplitude(float amplitude)
    {
        _amplitude = amplitude;
    }
    private float Ease(float time, float frequency, float strenght)
    {
        time = Mathf.Clamp01(time);

        return Mathf.Sin(time * frequency * Mathf.PI) * Mathf.Pow(1 - time, strenght);
    }

    public override void Evaluate(float time)
    {
        float frequency = 4f;

        float strenght = 1.8f;

        float ease = -Ease(time, frequency, strenght) * _amplitude;

        _amplitudeVector.x = ease;
        _amplitudeVector.y = ease;

        _transform.localScale = _vectorOne + _amplitudeVector;
    }
}

public class ShineAnimation : AnimatedData
{
    private Transform _shinePanel;

    private Image _shineImage;

    private Vector3 _vectorOne;

    private Vector3 _easingVector;

    private Color _shineColor;

    public ShineAnimation(Transform shinePanel, Image shineImage)
    {
        _shinePanel = shinePanel;

        _shineImage = shineImage;

        _shineColor = _shineImage.color;

        _vectorOne = Vector3.one;
    }

    public override void Evaluate(float time)
    {
        float size = LeanTween.easeOutQuad(1, 0, time) * 0.05f;

        //float shine = LeanTween.easeOutQuad(1, 0, time * 3);
        float shine = Mathf.Sin(time * Mathf.PI) * 0.2f;



        //// Shine Scale //
        //_easingVector.x = size;

        //_easingVector.y = size;

        //_shinePanel.localScale = _vectorOne - _easingVector;


        // Shine Color //
        _shineColor.a = shine;

        _shineImage.color = _shineColor;
    }
}

public class ColorChange : AnimatedData
{
    private Image _image;

    private Color _startColor;

    private Color _targetColor;

    public ColorChange(Image image, Color startColor, Color targetColor)
    {
        _image = image;

        _startColor = startColor;

        _targetColor = targetColor;
    }

    public override void Evaluate(float time)
    {
        float sinAnimation = Mathf.Sin(time * Mathf.PI);

        _image.color = Color.Lerp(_startColor, _targetColor, sinAnimation);
    }
}

public class CameraShake : AnimatedData
{
    private Camera _mainCamera;

    private Animator _marbleAnimator;

    private Vector3 _defaultLocalPosition;

    private Vector3 _directionMove;

    private float _defaultFieldOfView;

    public CameraShake(Camera mainCamera, Animator marbleAnimator)
    {
        _mainCamera = mainCamera;

        _marbleAnimator = marbleAnimator;

        _defaultFieldOfView = _mainCamera.fieldOfView;

        //_defaultLocalPosition = _mainCamera.transform.localPosition;
    }

    public void SetRandomPosition()
    {
        if(_defaultLocalPosition.Equals(Vector3.zero))
            _defaultLocalPosition = _mainCamera.transform.localPosition;

        _directionMove = Random.insideUnitCircle;
    }

    private float Easing(float time, float frequence, float height)
    {
        return Mathf.Sin(time * Mathf.PI * frequence) * Mathf.Pow(1 - time, height);
    }
    
    public override void Evaluate(float time)
    {
        _marbleAnimator.enabled = time > 0.8f;//

        time = Mathf.Clamp(time, 0, 0.8f);

        float bouncing = Easing(time, 4.9f, 1.5f);

        _mainCamera.fieldOfView = _defaultFieldOfView + bouncing * 1f;
        
        _mainCamera.transform.localPosition = _defaultLocalPosition + (_directionMove * bouncing * 0.12f);
    }
}

public class SineAnimate : AnimatedData
{
    private RectTransform _rectTransform;

    private float _amplitude = 0.1f;
    
    public SineAnimate(RectTransform rectTransform)
    {
        _rectTransform = rectTransform;
    }

    public override void Evaluate(float time)
    {
        float scale = Mathf.Sin(time * Mathf.PI) * _amplitude;

        _rectTransform.localScale = Vector3.one + (Vector3.one * scale);
    }
}

public abstract class AnimatedData
{
    public abstract void Evaluate(float time);
}
