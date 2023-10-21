using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    public event Action<Guid> SelectedMission;
    
    [SerializeField] private Transform _missionPointsContainer;
    [SerializeField] private MissionPoint _pointPrefab;

    private List<MissionPoint> _missionPoints;
    
    private MissionsStorage _storage;

    public void Initialize(MissionsStorage storage)
    {
        _storage = storage;
        CreateMissionPoints();
    }

    private void StateChanged(object sender, MissionStateChanged e)
    {
        _missionPoints.Find(x => x.Id == e.Mission.Id).SetState(e.State);
    }

    private void CreateMissionPoints()
    {
        _missionPoints = new List<MissionPoint>();
        
        foreach (var mission in _storage.Missions)
        {
            switch (mission)
            {
                case DualMissionDefinition dual:
                    _missionPoints.Add(CreateMissionPoint(dual.Mission1.Config, dual.Mission2.State));
                    _missionPoints.Add(CreateMissionPoint(dual.Mission2.Config, dual.Mission2.State));
                    break;
                case SingleMissionDefinition single:
                    _missionPoints.Add(CreateMissionPoint(single.Mission.Config, single.Mission.State));
                    break;
                default:
                    break;
            }
            
            mission.StateChanged += StateChanged;
        }
    }

    public MissionPoint CreateMissionPoint(MissionConfigSO config, MissionState initialState)
    {
        var missionPoint = Instantiate(_pointPrefab, _missionPointsContainer);
        missionPoint.Setup(config, initialState);
        missionPoint.transform.localPosition = config.Position;
        missionPoint.Selected += SelectMission;
        return missionPoint;
    }

    private void SelectMission(SelectableGameObject obj)
    {
        var missionPoint = (MissionPoint) obj;
        SelectedMission?.Invoke(missionPoint.Id);
    }
}