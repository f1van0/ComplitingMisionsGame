using System;

[Serializable]
public class Mission
{
    public MissionConfigSO Config;
    public MissionState State;

    public Mission(MissionConfigSO config, MissionState state)
    {
        Config = config;
        State = state;
    }
}