using System.Collections;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private MissionsContainerSO _missions;
    [SerializeField] private Map _map;
    [SerializeField] private MissionUI _ui;
    [SerializeField] private PlayerInputsService _inputsService;
    
    private CampaignProgression _campaignProgression;
    private MissionsStorage _missionsStorage;
    private HeroesStorage _heroesStorage;

    void Awake()
    {
        _inputsService.Initialize();
        
        _missionsStorage = new MissionsStorage(_missions);
        _heroesStorage = new HeroesStorage();
        _campaignProgression = new CampaignProgression(_missionsStorage, _heroesStorage);
        
        _map.Initialize(_missionsStorage);
        
        _map.SelectedMission += _missionsStorage.SelectMission;
        _missionsStorage.MissionSelected += _ui.ShowMissionPreview;
        
        _ui.MissionStarted += _missionsStorage.StartSelectedMission;
        _missionsStorage.MissionStarted += _ => _inputsService.DisableInputs();
        _missionsStorage.MissionStarted += _ui.ShowMissionAccomplishment;
        
        _ui.MissionCompleted += _missionsStorage.CompleteStartedMission;
        _missionsStorage.MissionCompleted += (_, __) => _inputsService.EnableInputs();
        _missionsStorage.MissionCompleted += _campaignProgression.CompleteMission;
        _missionsStorage.MissionCompleted += (_, __) => _ui.HideMission();
    }
}