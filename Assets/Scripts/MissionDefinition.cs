using System;
using Unity.VisualScripting;

public abstract class MissionDefinition
{
    public EventHandler<MissionStateChanged> StateChanged;
    public abstract bool ReferencesMission(Guid guid);
}

public class MissionStateChanged
{
    public MissionConfig Mission;
    public MissionState State;

    public MissionStateChanged(MissionConfig config, MissionState state)
    {
        Mission = config;
        State = state;
    }
}

public class SingleMissionDefinition : MissionDefinition
{
    public MissionConfig Config;
    public MissionState State;

    public SingleMissionDefinition(SingleMissionDefinitionSO missionDefinitionSo)
    {
        Config = missionDefinitionSo.config;
        State = missionDefinitionSo.initialState;
    }
    
    public override bool ReferencesMission(Guid guid)
    {
        return Config.Id == guid;
    }
    
    public void SetState(MissionState state)
    {
        State = state;
        StateChanged?.Invoke(this, new MissionStateChanged(Config, State));
    }
}

public class DualMissionDefinition : MissionDefinition
{
    public (MissionConfig config, MissionState state) Mission1;
    public (MissionConfig config, MissionState state) Mission2;
    
    public DualMissionDefinition(DualMissionDefinitionSO missionDefinitionSo)
    {
        Mission1.config = missionDefinitionSo.config1;
        Mission1.state = missionDefinitionSo.initialState;
        
        Mission2.config = missionDefinitionSo.config2;
        Mission2.state = missionDefinitionSo.initialState;
    }
    
    public override bool ReferencesMission(Guid guid)
    {
        return Mission1.config.Id == guid || Mission2.config.Id == guid;
    }
    
    public void SetState(Guid guid, MissionState state)
    {
        if (Mission1.config.Id == guid)
        {
            Mission1.state = state;
            StateChanged?.Invoke(this, new MissionStateChanged(Mission1.config, Mission1.state));

            if (state == MissionState.Completed)
            {
                Mission2.state = MissionState.Locked;
                StateChanged?.Invoke(this, new MissionStateChanged(Mission2.config, Mission1.state));
            }
        }
        else if (Mission2.config.Id == guid)
        {
            Mission2.state = state;
            StateChanged?.Invoke(this, new MissionStateChanged(Mission2.config, Mission2.state));

            if (state == MissionState.Completed)
            {
                Mission1.state = MissionState.Locked;
                StateChanged?.Invoke(this, new MissionStateChanged(Mission1.config, Mission1.state));
            }
        }
    }

    public MissionConfig GetConfig(Guid missionId)
    {
        if (Mission1.config.Id == missionId)
            return Mission1.config;
        else if (Mission2.config.Id == missionId)
            return Mission2.config;
        else
            throw new ArgumentOutOfRangeException(nameof(missionId));
    }
}