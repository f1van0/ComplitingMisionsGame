public class Hero
{
    public HeroType Type;
    public int Score;
    public bool IsUnlocked;

    public Hero(HeroType type, int score, bool isUnlocked)
    {
        Type = type;
        Score = score;
        IsUnlocked = isUnlocked;
    }
}