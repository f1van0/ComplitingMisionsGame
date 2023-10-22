using System;
using Unity.VisualScripting;
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
        MissionsStorage.SetState(mission, missionId, MissionState.Completed);
        UnlockMissions(mission);
        
        foreach (var missionDefinition in MissionsStorage.Missions)
        {
            if (missionDefinition.GetState() != MissionState.Unavailable)
                continue;
            
            if (!CheckIfMissionSuitsRequirements(missionDefinition)) 
                continue;
            
            MissionsStorage.SetState(missionDefinition, MissionState.Active);
            LockMissions(missionDefinition);
        }
    }

    private bool CheckIfMissionSuitsRequirements(MissionDefinition missionDefinition)
    {
        if (missionDefinition.Requirements == null)
            return true;
        
        foreach (var config in missionDefinition.Requirements)
        {
            if (MissionsStorage.GetMissionDefinition(config.Id).GetState() != MissionState.Completed)
                return false;
        }

        return true;
    }

    public void LockMissions(MissionDefinition mission)
    {
        foreach (var missionToBlock in mission.MissionsToBlockTemporarily)
        {
            MissionsStorage.SetState(missionToBlock, MissionState.Locked);
        }
    }

    public void UnlockMissions(MissionDefinition mission)
    {
        foreach (var missionToBlock in mission.MissionsToBlockTemporarily)
        {
            MissionsStorage.SetState(missionToBlock, MissionState.Active);
        }
    }
}