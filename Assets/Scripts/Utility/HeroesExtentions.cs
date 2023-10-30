using System;
using Data.Heroes;

namespace Utility
{
    public static class HeroesExtensions
    {
        public static string GetName(this HeroType type)
        {
            switch (type)
            {
                case HeroType.Current:
                    return "Текущий";
                case HeroType.Hawk:
                    return "Ястреб";
                case HeroType.Seagull:
                    return "Чайка";
                case HeroType.Owl:
                    return "Совух";
                case HeroType.Raven:
                    return "Ворон";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}