using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;


public class HeroesStorage
{
    private List<Hero> Heroes;

    public HeroesStorage()
    {
        Heroes = new List<Hero>();
    }

    public void AddHero(HeroType heroType)
    {
        if (TryGetHeroByType(heroType) != null)
            return;
        
        Heroes.Add(new Hero(heroType)); 
    }

    [CanBeNull]
    public Hero TryGetHeroByType(HeroType heroType)
    {
        return Heroes.FirstOrDefault(x => x.Type == heroType);
    }
}