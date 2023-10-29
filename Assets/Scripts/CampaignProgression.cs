using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CampaignProgression
{
    public event Action<MissionDefinition, Guid> MissionSelected;
    public event Action<MissionConfigSO> MissionStarted;
    public event Action<MissionDefinition, Guid> MissionCompleted;
    
    public readonly MissionsStorage MissionsStorage;
    public readonly HeroesStorage HeroesStorage;
    
    private MissionDefinition _currentMissionDefinition;
    private MissionConfigSO _currentMissionConfig;
    private Guid _currentMissionId;

    private Hero _selectedHero;

    public CampaignProgression(MissionsStorage missionsStorage, HeroesStorage heroesStorage)
    {
        MissionsStorage = missionsStorage;
        HeroesStorage = heroesStorage;
    }
    
    public void SelectMission(Guid missionId)
    {
        var selectedMissionDefinition = MissionsStorage.GetMissionDefinition(missionId);
        if (selectedMissionDefinition.GetState() == MissionState.Completed)
            return;

        _currentMissionDefinition = selectedMissionDefinition;
        _currentMissionId = missionId;
        MissionSelected?.Invoke(_currentMissionDefinition, _currentMissionId);
    }

    public void SelectHero(HeroType type)
    {
        if (HeroesStorage.TryGetHeroByType(type, out var hero))
        {
            _selectedHero = hero;
        }
        else
        {
            throw new Exception("The Hero you want to choose is not exists in storage.");
        }
    }

    public void StartSelectedMission(Guid missionId)
    {
        if (_selectedHero == null)
        {
            return;
        }
        
        if (_currentMissionDefinition != MissionsStorage.GetMissionDefinition(missionId))
        {
            throw new Exception($"The Id of the mission to be started does not belong to the currently selected mission");
        }
        
        _currentMissionId = missionId;
        _currentMissionConfig = MissionsStorage.GetMissionConfig(_currentMissionId);
        
        MissionStarted?.Invoke(_currentMissionConfig);
    }

    public void CompleteStartedMission()
    {
        if (_currentMissionDefinition == null || _currentMissionId == Guid.Empty)
        {
            throw new Exception($"Failed to complete a mission which was not started");
        }

        CompleteCurrentMission();
        MissionCompleted?.Invoke(_currentMissionDefinition, _currentMissionId);
        _currentMissionDefinition = null;
        _currentMissionId = Guid.Empty;
    }

    public void CompleteCurrentMission()
    {
        MissionsStorage.SetState(_currentMissionDefinition, _currentMissionId, MissionState.Completed);
        UnlockMissions(_currentMissionDefinition);
        HeroesStorage.UnlockHeroes(_currentMissionConfig.UnlockingHeroes);
        UpdateHeroStats(_currentMissionConfig.HeroPoints);
        
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

    private void UpdateHeroStats(List<InspectorKeyValue<HeroType, int>> heroPoints)
    {
        var copiedList = new List<InspectorKeyValue<HeroType, int>>(heroPoints);
        foreach (var heroPoint in copiedList)
        {
            if (heroPoint.Key == HeroType.Current)
            {
                heroPoint.Key = _selectedHero.Type;
            }
        }

        HeroesStorage.ChangeStats(copiedList);
    }

    private bool CheckIfMissionSuitsRequirements(MissionDefinition missionDefinition)
    {
        if (missionDefinition.Requirements == null)
            return true;

        MissionDefinition desiredMD;
        foreach (var config in missionDefinition.Requirements)
        {
            desiredMD = MissionsStorage.GetMissionDefinition(config.Id);
            if (desiredMD.GetState() != MissionState.Completed)
            {
                if (!MissionsStorage.HasCompletedDualMissionInAncestors(desiredMD))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void LockMissions(MissionDefinition mission)
    {
        foreach (var missionToBlock in mission.MissionsToBlockTemporarily)
        {
            MissionsStorage.SetState(missionToBlock, MissionState.Locked);
        }
    }

    private void UnlockMissions(MissionDefinition mission)
    {
        foreach (var missionToBlock in mission.MissionsToBlockTemporarily)
        {
            MissionsStorage.SetState(missionToBlock, MissionState.Active);
        }
    }
}