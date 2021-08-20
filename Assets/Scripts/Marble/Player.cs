using UnityEngine;

public class Player : MonoBehaviour
{
    public LevelInfo LevelInfo { get; private set; }

    private void Awake()
    {
        LevelInfo = new LevelInfo();
    }

    public void LevelFinish()
    {
        LevelInfo.AddLevel();
    }

    public void AddClick()
    {
        LevelInfo.AddClick();
    }
}
