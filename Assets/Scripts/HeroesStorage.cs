using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;


public class HeroesStorage
{
    public event Action<Hero> HeroAdded;
    public event Action<Hero> HeroDataUpdated;
    
    private List<Hero> Heroes;

    public HeroesStorage()
    {
        Heroes = new List<Hero>();
        SetHeroData(HeroType.Hawk, 0, true);
    }

    public void SetHeroData(HeroType heroType, int score, bool isUnlocking)
    {
        Hero hero;
        if (TryGetHeroByType(heroType, out hero))
        {
            if (isUnlocking)
                hero.IsUnlocked = true;
            
            hero.Score += score;
            
        }
        else
        {
            hero = new Hero(heroType, score, isUnlocking);
            Heroes.Add(hero);
        }
        
        HeroDataUpdated?.Invoke(hero);
    }
    
    public void UnlockHero(HeroType heroType)
    {
        if (heroType == HeroType.Current)
            return;
        
        SetHeroData(heroType, 0, true);
    }

    [CanBeNull]
    public bool TryGetHeroByType(HeroType heroType, out Hero hero)
    {
        hero = Heroes.FirstOrDefault(x => x.Type == heroType);
        return hero != null;
    }

    public void UnlockHeroes(List<HeroType> heroes)
    {
        foreach (var hero in heroes)
        {
            UnlockHero(hero);
        }
    }

    public void ChangeStats(List<InspectorKeyValue<HeroType, int>> heroPoints)
    {
        foreach (var pointsForHero in heroPoints)
        {
            SetHeroData(pointsForHero.Key, pointsForHero.Value, false);
        }
    }

    public List<Hero> GetUnlockedHeroes()
    {
        return Heroes.FindAll(x => x.IsUnlocked == true);
    }
}