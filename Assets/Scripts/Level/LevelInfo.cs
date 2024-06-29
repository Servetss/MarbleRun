
public class LevelInfo
{
    public bool IsWin;

    public int PlayerLevel { get; private set; }

    public int CoinsGetOnTheLevel { get; private set; }

    public int BoostClickCount { get; private set; }

    public float Boost { get; private set; }

    public int Speed { get; private set; }

    public void AddLevel()
    {
        PlayerLevel++;
    }

    public void AddClick()
    {
        BoostClickCount++;
    }

    public void AddCoin()
    {
        CoinsGetOnTheLevel++;
    }

    public void ResetCoins()
    {
        CoinsGetOnTheLevel = 0;
    }

    public void SetSpeed(int speed)
    {
        Speed = speed;
    }

    public void SetBoost(float boost)
    {
        Boost = boost;
    }
}
