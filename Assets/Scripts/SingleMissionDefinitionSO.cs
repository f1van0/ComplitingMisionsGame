using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Missions/SingleMissionDefinitionAsset", fileName = "SingleMissionDefinitionAsset", order = 2)]
public class SingleMissionDefinitionSO : MissionDefinitionSO
{
    public MissionConfig config;
    public MissionState initialState = MissionState.Unavailable;
}