namespace Data.Missions
{
    public enum TransitionStatus
    {
        /// связь с миссией которая по всем признакам могла быть пройдена, но её статус Disabled
        Unvisited,
        /// связб с миссией которую нельзя пройти по причине исключающего выбора
        DisabledByChoice,
        /// связь с миссией которая была пройдена
        Completed
    }
}