using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;

namespace Data.Missions
{
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

        public void SetMissionState(MissionDefinition mission, Guid missionId, MissionState state)
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

        public void SetMissionState(MissionDefinition mission, MissionState state)
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

        public void SetMissionState(MissionConfigSO config, MissionState state)
        {
            var missionDefinition = GetMissionDefinition(config.Id);
            SetMissionState(missionDefinition, state);
        }

        public TransitionStatus GetTransitionStatus(Guid missionId)
        {
            MissionDefinition mission = GetMissionDefinition(missionId);
            MissionState state;
            switch (mission)
            {
                case SingleMissionDefinition single:
                    state = single.GetMissionState();
                    break;
                case DualMissionDefinition dual:
                    if (dual.IsDisabledByChoice(missionId))
                    {
                        return TransitionStatus.DisabledByChoice;
                    }
                    state = dual.GetMissionState(missionId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mission));
            }

            if (state == MissionState.Completed)
            {
                return TransitionStatus.Completed;
            }

            if (state == MissionState.Active)
            {
                return TransitionStatus.Unvisited;
            }

            return AnyParentIsUnvisited(mission);
        }

        private TransitionStatus AnyParentIsUnvisited(MissionDefinition mission)
        {
            var statuses = mission.Requirements.Select(r => GetTransitionStatus(r.Id)).ToArray();

            // can be unvisited or unvisited and locked
            if (statuses.Any(s => s == TransitionStatus.Unvisited))
            {
                return TransitionStatus.Unvisited;
            }

            if (statuses.Any(s => s == TransitionStatus.DisabledByChoice))
            {
                return TransitionStatus.DisabledByChoice;
            }

            throw new InvalidOperationException("Parent was in incorrect state. Check transition logic");
        }
    }
}