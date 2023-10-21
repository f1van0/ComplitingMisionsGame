public class Hero
{
    public HeroType Type;
    public int Score;

    public Hero(HeroType type)
    {
        Type = type;
    }
    
    public Hero(HeroType type, int score)
    {
        Type = type;
        Score = score;
    }
}