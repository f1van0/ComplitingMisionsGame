using System;
using System.Collections.Generic;
using System.Linq;
using Data.Missions;
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

    private HeroData _selectedHero;

    public CampaignProgression(MissionsStorage missionsStorage, HeroesStorage heroesStorage)
    {
        MissionsStorage = missionsStorage;
        HeroesStorage = heroesStorage;
    }

    public void SelectMission(Guid missionId)
    {
        var selectedMissionDefinition = MissionsStorage.GetMissionDefinition(missionId);
        if (selectedMissionDefinition.GetMissionState() != MissionState.Active)
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
            throw new Exception(
                $"The Id of the mission to be started does not belong to the currently selected mission");
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
        if (_currentMissionDefinition.GetMissionState() == MissionState.Completed)
            throw new Exception("Attempting to complete a mission that has already been completed");

        MissionsStorage.SetMissionState(_currentMissionDefinition, _currentMissionId, MissionState.Completed);
        UnlockMissions(_currentMissionDefinition);
        HeroesStorage.UnlockHeroes(_currentMissionConfig.UnlockingHeroes);
        UpdateHeroStats(_currentMissionConfig.HeroPoints);

        foreach (var missionDefinition in MissionsStorage.Missions)
        {
            if (missionDefinition.GetMissionState() != MissionState.Unavailable)
                continue;

            if (!CanEnterMission(missionDefinition))
                continue;

            MissionsStorage.SetMissionState(missionDefinition, MissionState.Active);
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

    private bool CanEnterMission(MissionDefinition missionDefinition)
    {
        if (missionDefinition.Requirements == null)
            return true;

        // check if all parents are completed or disabled by choice
        TransitionStatus[] transitionStatuses = missionDefinition.Requirements
            .Select(GetParentTransitionStatus)
            .ToArray();

        if (transitionStatuses.Contains(TransitionStatus.Unvisited))
        {
            return false;
        }

        if (!transitionStatuses.Contains(TransitionStatus.Completed))
        {
            return false;
        }

        return true;
    }

    private TransitionStatus GetParentTransitionStatus(MissionConfigSO config)
    {
        Guid missionId = config.Id;
        MissionDefinition parentMission = MissionsStorage.GetMissionDefinition(missionId);
        MissionState desiredState;
        switch (parentMission)
        {
            case DualMissionDefinition dualMissionDefinition:
                desiredState = dualMissionDefinition.GetMissionState(missionId);
                break;
            case SingleMissionDefinition singleMissionDefinition:
                desiredState = singleMissionDefinition.GetMissionState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(parentMission));
        }

        if (desiredState == MissionState.Completed)
        {
            return TransitionStatus.Completed;
        }

        return MissionsStorage.GetTransitionStatus(missionId);
    }

    private void LockMissions(MissionDefinition mission)
    {
        MutateMissionState(mission, MissionState.Locked);
    }

    private void UnlockMissions(MissionDefinition mission)
    {
        MutateMissionState(mission, MissionState.Active);
    }

    private void MutateMissionState(MissionDefinition mission, MissionState state)
    {
        foreach (var missionToBlock in mission.MissionsToBlockTemporarily)
        {
            var mdToBlock = MissionsStorage.GetMissionDefinition(missionToBlock.Id);
            if (mdToBlock.GetMissionState() != MissionState.Completed)
                MissionsStorage.SetMissionState(missionToBlock, state);
        }
    }
}