using System;
using System.Collections.Generic;
using Data.Heroes;
using UnityEngine;
using Utility;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Missions/MissionConfig", fileName = "MissionConfigAsset", order = 3)]
    public class MissionConfigSO : ScriptableObject
    {
        public Guid Id = Guid.NewGuid();

        public string Number;
        public string Name;
        [Multiline]
        public string Foreword;
        [Multiline]
        public string Description;
        public string PlayerSide;
        public string EnemySide;

        [Space]
        public Vector2 Position;
    
        [Space]
        public List<HeroType> UnlockingHeroes;
        public List<InspectorKeyValue<HeroType, int>> HeroPoints;
    }
}