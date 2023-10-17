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
    
    private MissionsContainer _container;

    public void Initialize(MissionsContainer container)
    {
        _container = container;
        CreateMissionPoints();
    }

    private void StateChanged(object sender, MissionStateChanged e)
    {
        _missionPoints.Find(x => x.Id == e.Mission.Id).SetState(e.State);
    }

    private void CreateMissionPoints()
    {
        _missionPoints = new List<MissionPoint>();
        
        foreach (var mission in _container.Missions)
        {
            switch (mission)
            {
                case DualMissionDefinition dualMissionDefinition:
                    _missionPoints.Add(CreateMissionPoint(dualMissionDefinition.Mission1.config, dualMissionDefinition.Mission2.state));
                    _missionPoints.Add(CreateMissionPoint(dualMissionDefinition.Mission2.config, dualMissionDefinition.Mission2.state));
                    break;
                case SingleMissionDefinition singleMissionDefinition:
                    _missionPoints.Add(CreateMissionPoint(singleMissionDefinition.Config, singleMissionDefinition.State));
                    break;
                default:
                    break;
            }
            
            mission.StateChanged += StateChanged;
        }
    }

    public MissionPoint CreateMissionPoint(MissionConfig config, MissionState initialState)
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