
public class LevelInfo
{
    public int PlayerLevel { get; private set; }

    public int CoinsGetOnTheLevel { get; private set; }

    public int BoostClickCount { get; private set; }

    public void AddLevel()
    {
        PlayerLevel++;
    }

    public void AddClick()
    {
        BoostClickCount++;
    }
}
