using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionsStorage
{
    public List<MissionDefinition> Missions;

    public MissionsStorage(MissionsContainerSO container)
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

    public MissionDefinition GetMissionDefinition(Guid missionId)
    {
        return Missions.Find(x => x.ReferencesMission(missionId));
    }
    
    public MissionConfigSO GetMissionConfig(Guid missionId)
    {
        var missionDefinition = Missions.Find(x => x.ReferencesMission(missionId));
        switch (missionDefinition)
        {
            case SingleMissionDefinition singleMissionDefinition:
                return singleMissionDefinition.MissionData.Config;
            case DualMissionDefinition dualMissionDefinition:
                return dualMissionDefinition.GetConfig(missionId);
            default:
                throw new ArgumentOutOfRangeException(nameof(missionId));
        }
    }

    public void SetState(MissionDefinition mission, Guid missionId, MissionState state)
    {
        switch (mission)
        {
            case DualMissionDefinition dualMissionData:
                dualMissionData.SetState(missionId, state);
                break;
            case SingleMissionDefinition singleMissionData:
                singleMissionData.SetState(state);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mission));
        }
    }

    public void SetState(MissionDefinition mission, MissionState state)
    {
        switch (mission)
        {
            case DualMissionDefinition dual:
                dual.SetState(dual.Mission1.Config.Id, state);
                dual.SetState(dual.Mission2.Config.Id, state);
                break;
            case SingleMissionDefinition single:
                single.SetState(state);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetState(MissionConfigSO config, MissionState state)
    {
        var missionDefinition = GetMissionDefinition(config.Id);
        SetState(missionDefinition, state);
    }

    public bool HasCompletedDualMissionInAncestors(MissionDefinition mission)
    {
        if (!Missions.Contains(mission))
            throw new Exception("The HasCompletedDualMissionInAncestors method received a mission that do not exist in the storage");

        MissionDefinition desiredMD;
        foreach (var config in mission.Requirements)
        {
            desiredMD = GetMissionDefinition(config.Id);
            if (desiredMD is DualMissionDefinition dualMission 
                && dualMission.GetState() == MissionState.Completed
                && dualMission.GetState(config.Id) != MissionState.Completed)
            {
                return true;
            }
            else
            {
                var hasCompletedDualMissionInAncestors = HasCompletedDualMissionInAncestors(desiredMD);
                if (hasCompletedDualMissionInAncestors)
                    return true;
            }
        }

        return false;
    }
}