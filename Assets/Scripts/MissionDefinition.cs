using System;
using Unity.VisualScripting;

public abstract class MissionDefinition
{
    public EventHandler<MissionStateChanged> StateChanged;
    public abstract bool ReferencesMission(Guid guid);
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
    public Mission Mission;

    public SingleMissionDefinition(SingleMissionDefinitionSO missionDefinitionSo)
    {
        Mission = new Mission(missionDefinitionSo.config, missionDefinitionSo.initialState);
    }
    
    public override bool ReferencesMission(Guid guid)
    {
        return Mission.Config.Id == guid;
    }
    
    public void SetState(MissionState state)
    {
        Mission.State = state;
        StateChanged?.Invoke(this, new MissionStateChanged(Mission.Config, Mission.State));
    }
}

public class DualMissionDefinition : MissionDefinition
{
    public Mission Mission1;
    public Mission Mission2;
    
    public DualMissionDefinition(DualMissionDefinitionSO missionDefinitionSo)
    {
        Mission1.Config = missionDefinitionSo.config1;
        Mission1.State = missionDefinitionSo.initialState;
        
        Mission2.Config = missionDefinitionSo.config2;
        Mission2.State = missionDefinitionSo.initialState;
    }
    
    public override bool ReferencesMission(Guid guid)
    {
        return Mission1.Config.Id == guid || Mission2.Config.Id == guid;
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
                StateChanged?.Invoke(this, new MissionStateChanged(Mission2.Config, Mission1.State));
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