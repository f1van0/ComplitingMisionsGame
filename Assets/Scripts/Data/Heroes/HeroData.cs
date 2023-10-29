public class HeroData
{
    public HeroType Type;
    public int Score;
    public bool IsUnlocked;

    public HeroData(HeroType type, int score, bool isUnlocked)
    {
        Type = type;
        Score = score;
        IsUnlocked = isUnlocked;
    }
}