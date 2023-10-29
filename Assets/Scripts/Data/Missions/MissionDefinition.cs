using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class MissionDefinition
{
    public List<MissionConfigSO> Requirements; // список миссий, которые мы должны пройти, чтобы разблокировать текущую
    public List<MissionConfigSO> MissionsToBlockTemporarily; //список миссий, которые мы заблокируем после того, как эта миссия разблокируется и разблокируем, когда пройдём эту миссию
    
    public EventHandler<MissionStateChanged> StateChanged;
    public abstract bool ReferencesMission(Guid guid);
    public abstract MissionState GetState();
}

public class MissionStateChanged
{
    public MissionConfigSO Mission;
    public MissionState State;

    public MissionStateChanged(MissionConfigSO config, MissionState state)
    {
        Mission = config;
        State = state;
    }
}

public class SingleMissionDefinition : MissionDefinition
{
    public MissionData MissionData;

    public SingleMissionDefinition(SingleMissionDefinitionSO missionDefinitionSo)
    {
        MissionsToBlockTemporarily = missionDefinitionSo.MissionsToBlockTemporarily;
        Requirements = missionDefinitionSo.Requirements;
        MissionData = new MissionData(missionDefinitionSo.config, missionDefinitionSo.initialState);
    }
    
    public override bool ReferencesMission(Guid guid)
    {
        return MissionData.Config.Id == guid;
    }

    public override MissionState GetState()
    {
        return MissionData.State;
    }

    public void SetState(MissionState state)
    {
        MissionData.State = state;
        StateChanged?.Invoke(this, new MissionStateChanged(MissionData.Config, MissionData.State));
    }
}

public class DualMissionDefinition : MissionDefinition
{
    public MissionData Mission1;
    public MissionData Mission2;
    
    public DualMissionDefinition(DualMissionDefinitionSO missionDefinitionSo)
    {
        MissionsToBlockTemporarily = missionDefinitionSo.MissionsToBlockTemporarily;
        Requirements = missionDefinitionSo.Requirements;
        Mission1 = new MissionData(missionDefinitionSo.config1, missionDefinitionSo.initialState);
        Mission2 = new MissionData(missionDefinitionSo.config2, missionDefinitionSo.initialState);
    }
    
    public override bool ReferencesMission(Guid guid)
    {
        return Mission1.Config.Id == guid || Mission2.Config.Id == guid;
    }
    
    public override MissionState GetState()
    {
        if (Mission1.State == Mission2.State)
        {
            return Mission1.State;
        }
        else
        {
            if (Mission1.State == MissionState.Completed || Mission2.State == MissionState.Completed)
                return MissionState.Completed;
        }
        
        return MissionState.Locked;
    }

    public MissionState GetState(Guid id)
    {
        if (Mission1.Config.Id == id)
            return Mission1.State;
        else if (Mission2.Config.Id == id)
            return Mission2.State;
        else
            throw new Exception("Cannot get a mission state that is not in DualMissionDefinition");
    }

    public void SetState(MissionState state)
    {
        Mission1.State = state;
        Mission2.State = state;
        StateChanged?.Invoke(this, new MissionStateChanged(Mission1.Config, Mission2.State));
        StateChanged?.Invoke(this, new MissionStateChanged(Mission2.Config, Mission2.State));
    }
    
    public void SetState(Guid guid, MissionState state)
    {
        if (Mission1.Config.Id == guid)
        {
            Mission1.State = state;
            StateChanged?.Invoke(this, new MissionStateChanged(Mission1.Config, Mission1.State));

            if (state == MissionState.Completed)
            {
                Mission2.State = MissionState.Locked;
                StateChanged?.Invoke(this, new MissionStateChanged(Mission2.Config, Mission2.State));
            }
        }
        else if (Mission2.Config.Id == guid)
        {
            Mission2.State = state;
            StateChanged?.Invoke(this, new MissionStateChanged(Mission2.Config, Mission2.State));

            if (state == MissionState.Completed)
            {
                Mission1.State = MissionState.Locked;
                StateChanged?.Invoke(this, new MissionStateChanged(Mission1.Config, Mission1.State));
            }
        }
    }

    public MissionConfigSO GetConfig(Guid missionId)
    {
        if (Mission1.Config.Id == missionId)
            return Mission1.Config;
        else if (Mission2.Config.Id == missionId)
            return Mission2.Config;
        else
            throw new ArgumentOutOfRangeException(nameof(missionId));
    }
}