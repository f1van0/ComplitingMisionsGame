using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class MissionDefinitionSO : ScriptableObject
    {
        public List<MissionConfigSO> Requirements; // список миссий, которые мы должны пройти, чтобы разблокировать текущую
        public List<MissionConfigSO> MissionsToBlockTemporarily; //список миссий, которые мы заблокируем после того, как эта миссия разблокируется и разблокируем, когда пройдём эту миссию
    }
}