using Data.Missions;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Missions/DualMissionDefinition", fileName = "DualMissionDefinitionAsset", order = 2)]
    public class DualMissionDefinitionSO : MissionDefinitionSO
    {
        public MissionConfigSO config1;
        public MissionConfigSO config2;
        public MissionState initialState;
    }
}