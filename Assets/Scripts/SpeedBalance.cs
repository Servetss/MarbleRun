using UnityEngine;

public static class SpeedBalance
{
    public static float StartSpeed { get => 45; }

    public static float SpeedIncrease { get => 20; }

    public static float CalculatePlayerSppedByLevel(int level)
    {
        level++;
        
        return Mathf.Sqrt(level * 0.1f);
    }

    public static float CalculateEnemySpeedByLevel(int level)
    {
        level++;
        
        float cof = level <= 10 ? 3.2f : 3f;   // diference speed for first 10 levels
        
        return Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(level * 0.1f, cof)));
    }
}
