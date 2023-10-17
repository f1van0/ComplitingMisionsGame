using UnityEngine;

[CreateAssetMenu(menuName = "Missions/DualMissionDefinitionAsset", fileName = "DualMissionDefinitionAsset", order = 2)]
public class DualMissionDefinitionSO : MissionDefinitionSO
{
    public MissionConfig config1;
    public MissionConfig config2;
    public MissionState initialState;
}