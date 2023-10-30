using System;
using System.Collections.Generic;
using System.Linq;
using Data.Heroes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Heroes
{
    public class HeroesUI : MonoBehaviour
    {
        public event Action<HeroType> HeroSelected;
    
        [SerializeField] private ToggleGroup _heroesGroup;
        [SerializeField] private HeroCardUI _heroCardPrefab;

        private List<HeroCardUI> _heroCards = new List<HeroCardUI>();

        public void Initialize(HeroesStorage heroesStorage)
        {
            foreach (var unlockedHero in heroesStorage.GetUnlockedHeroes())
            {
                SetHeroData(unlockedHero);
            }
        }

        public void AddHero(HeroData heroData)
        {
            var heroCard = Instantiate(_heroCardPrefab, _heroesGroup.transform);
            heroCard.Setup(heroData, _heroesGroup);
            _heroCards.Add(heroCard);
            heroCard.StateChanged += ChangeSelectedHero;
        }

        public void SetHeroData(HeroData heroData)
        {
            if (heroData.IsUnlocked == false)
                return;
        
            var foundHero = _heroCards.FirstOrDefault(x => x.HeroType == heroData.Type);

            if (foundHero == null)
            {
                AddHero(heroData);
            }
            else
            {
                foundHero.SetValues(heroData);
            }
        }

        public void ChangeSelectedHero()
        {
            var selectedHero = _heroesGroup.GetFirstActiveToggle().GetComponent<HeroCardUI>();
            HeroSelected?.Invoke(selectedHero.HeroType);
        }

        private void OnDestroy()
        {
            foreach (var heroCard in _heroCards)
            {
                heroCard.StateChanged -= ChangeSelectedHero;
            }
        }

        public void DisableSelecting()
        {
            foreach (var heroCard in _heroCards)
            {
                heroCard.SetInteractivity(false);
            }
        }

        public void EnableSelecting()
        {
            foreach (var heroCard in _heroCards)
            {
                heroCard.SetInteractivity(true);
            }
        }
    }
}