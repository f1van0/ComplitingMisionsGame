using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Missions/SingleMissionDefinition", fileName = "SingleMissionDefinitionAsset", order = 2)]
public class SingleMissionDefinitionSO : MissionDefinitionSO
{
    public MissionConfigSO config;
    public MissionState initialState = MissionState.Unavailable;
}