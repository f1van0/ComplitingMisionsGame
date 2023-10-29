public enum MissionState
{
    /// видно и можно пройти
    Active = 0,
    /// невидимая которую ещё не открыли
    Unavailable,
    /// временно заблокированная
    Locked,
    /// пройденная миссия
    Completed
}