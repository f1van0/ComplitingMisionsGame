using Data.Missions;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Missions/SingleMissionDefinition", fileName = "SingleMissionDefinitionAsset", order = 2)]
    public class SingleMissionDefinitionSO : MissionDefinitionSO
    {
        public MissionConfigSO config;
        public MissionState initialState = MissionState.Unavailable;
    }
}