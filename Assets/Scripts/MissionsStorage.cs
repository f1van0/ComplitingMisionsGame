using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionsStorage
{
    public event Action<MissionDefinition, Guid> MissionSelected;
    public event Action<MissionConfigSO> MissionStarted;
    public event Action<MissionDefinition, Guid> MissionCompleted;
    
    public List<MissionDefinition> Missions;
    
    public MissionDefinition CurrentMissionDefinition;
    public MissionConfigSO CurrentMissionConfig;
    public Guid CurrentMissionId;

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

    public void SelectMission(Guid missionId)
    {
        CurrentMissionDefinition = GetMissionDefinition(missionId);
        CurrentMissionId = missionId;
        MissionSelected?.Invoke(CurrentMissionDefinition, CurrentMissionId);
    }

    public void StartSelectedMission(Guid missionId)
    {
        if (CurrentMissionDefinition != GetMissionDefinition(missionId))
        {
            throw new Exception($"The Id of the mission to be started does not belong to the currently selected mission");
        }
        
        CurrentMissionId = missionId;
        CurrentMissionConfig = GetMissionConfig(CurrentMissionId);
        
        MissionStarted?.Invoke(CurrentMissionConfig);
    }

    public void CompleteStartedMission(Guid missionGuid)
    {
        if (CurrentMissionId != missionGuid)
        {
            throw new Exception($"The calling code tries to end a mission that has not been selected");
        }
        
        MissionCompleted?.Invoke(CurrentMissionDefinition, CurrentMissionId);
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
                return singleMissionDefinition.Mission.Config;
            case DualMissionDefinition dualMissionDefinition:
                return dualMissionDefinition.GetConfig(missionId);
            default:
                throw new ArgumentOutOfRangeException(nameof(missionId));
        }
    }
}