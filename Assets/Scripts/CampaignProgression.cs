using System;
using UnityEngine;

public class CampaignProgression
{

    public readonly MissionsStorage MissionsStorage;
    public readonly HeroesStorage HeroesStorage;

    public CampaignProgression(MissionsStorage missionsStorage, HeroesStorage heroesStorage)
    {
        MissionsStorage = missionsStorage;
        HeroesStorage = heroesStorage;
    }

    public void CompleteMission(MissionDefinition mission, Guid missionId)
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