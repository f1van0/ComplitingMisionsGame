using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/MissionsContainer", fileName = "MissionsContainerAsset", order = 1)]
public class MissionsContainerSO : ScriptableObject
{
    public List<MissionDefinitionSO> missions;
}