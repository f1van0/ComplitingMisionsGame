namespace Data.Missions
{
    public enum MissionState
    {
        /// видно и можно пройти
        Active = 0,
        /// невидимая, которую ещё не открыли или заблокированный вариант двойной миссии
        Unavailable,
        /// временно заблокированная
        Locked,
        /// пройденная миссия
        Completed
    }
}