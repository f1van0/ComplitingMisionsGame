using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MissionDefinitionSO : ScriptableObject
{
    public List<MissionConfigSO> requirement; // список миссий, которые мы должны пройти, чтобы разблокировать текущую
    public List<MissionConfigSO> missionsToShow; //список миссий, которые мы отобразим после того, как эта миссия разблокируется
}