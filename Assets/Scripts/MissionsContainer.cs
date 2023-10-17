using System.Collections.Generic;
using UnityEngine;

public class MissionsContainer
{
    public List<MissionDefinition> Missions;

    public MissionsContainer(MissionsContainerSO container)
    {
        Missions = new List<MissionDefinition>();
        
        foreach (var mission in container.missions)
        {
            switch (mission)
            {
                case SingleMissionDefinitionSO singleMission:
                    Missions.Add(new SingleMissionDefinition(singleMission));
                    break;
                case DualMissionDefinitionSO dualMission:
                    Missions.Add(new DualMissionDefinition(dualMission));
                    break;
                default:
                    break;
            }
        }
    }
}