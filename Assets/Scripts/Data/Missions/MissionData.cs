using System;

[Serializable]
public class MissionData
{
    public MissionConfigSO Config;
    public MissionState State;

    public MissionData(MissionConfigSO config, MissionState state)
    {
        Config = config;
        State = state;
    }
}