using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;


public class HeroesStorage
{
    public event Action<HeroData> HeroAdded;
    public event Action<HeroData> HeroDataUpdated;
    
    private List<HeroData> Heroes;

    public HeroesStorage()
    {
        Heroes = new List<HeroData>();
        SetHeroData(HeroType.Hawk, 0, true);
    }

    public void SetHeroData(HeroType heroType, int score, bool isUnlocking)
    {
        HeroData heroData;
        if (TryGetHeroByType(heroType, out heroData))
        {
            if (isUnlocking)
                heroData.IsUnlocked = true;
            
            heroData.Score += score;
            
        }
        else
        {
            heroData = new HeroData(heroType, score, isUnlocking);
            Heroes.Add(heroData);
        }
        
        HeroDataUpdated?.Invoke(heroData);
    }
    
    public void UnlockHero(HeroType heroType)
    {
        if (heroType == HeroType.Current)
            return;
        
        SetHeroData(heroType, 0, true);
    }

    public bool TryGetHeroByType(HeroType heroType, out HeroData heroData)
    {
        heroData = Heroes.FirstOrDefault(x => x.Type == heroType);
        return heroData != null;
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

    public List<HeroData> GetUnlockedHeroes()
    {
        return Heroes.FindAll(x => x.IsUnlocked == true);
    }
}