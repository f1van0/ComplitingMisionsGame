using System;
using UnityEngine;

public class CampaignProgression
{
    public readonly MissionsContainer MissionsContainer;

    public CampaignProgression(MissionsContainerSO missions)
    {
        MissionsContainer = new MissionsContainer(missions);
    }

    public void CompleteMission(Guid missionId)
    {
        var mission = MissionsContainer.Missions.Find(x => x.ReferencesMission(missionId));
        CompleteMission(mission, missionId);
    }

    public MissionConfig GetMissionConfig(Guid missionId)
    {
        var missionDefinition = MissionsContainer.Missions.Find(x => x.ReferencesMission(missionId));
        switch (missionDefinition)
        {
            case SingleMissionDefinition singleMissionDefinition:
                return singleMissionDefinition.Config;
            case DualMissionDefinition dualMissionDefinition:
                return dualMissionDefinition.GetConfig(missionId);
            default:
                throw new ArgumentOutOfRangeException(nameof(missionId));
        }
    }

    private void CompleteMission(MissionDefinition mission, Guid missionId)
    {
        switch (mission)
        {
            case DualMissionDefinition dualMissionData:
                dualMissionData.SetState(missionId, MissionState.Completed);
                break;
            case SingleMissionDefinition singleMissionData:
                singleMissionData.SetState(MissionState.Completed);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mission));
        }
    }
}