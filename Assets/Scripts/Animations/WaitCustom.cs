using System.Collections.Generic;
using UnityEngine;

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

            if (_actualStructInLoop.Time >= 1)
            {
                OnFinish(i);
                
                i--;
            }
        }
    }

    private void OnFinish(int animationDataIndex)
    {
        _animationStructList.RemoveAt(animationDataIndex);
    }
    
    public WaitCustom Play(AnimatedData animData, float speed)
    {
        int itemIndex = IsAnimationPlayed(animData);

        if (itemIndex == -1)
        {
            AnimationStruct animationStruct = new AnimationStruct(animData, speed);

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

    private int IsAnimationPlayed(AnimatedData animatedData)
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

    public AnimationStruct(AnimatedData animatedData, float speed)
    {
        AnimatedData = animatedData;

        Speed = speed;
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

public abstract class AnimatedData
{
    public abstract void Evaluate(float time);
}
