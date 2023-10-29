using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Startup : MonoBehaviour
{
    [SerializeField] private MissionsContainerSO _missions;
    [SerializeField] private Map _map;
    [SerializeField] private MissionUI _missionUI;
    [SerializeField] private HeroesUI _heroesUI;
    [SerializeField] private PlayerInputsService _inputsService;
    
    private CampaignProgression _campaignProgression;
    private MissionsStorage _missionsStorage;
    private HeroesStorage _heroesStorage;

    void Awake()
    {
        _missionsStorage = new MissionsStorage(_missions);
        _heroesStorage = new HeroesStorage();
        _campaignProgression = new CampaignProgression(_missionsStorage, _heroesStorage);
        
        _map.SelectedMission += _campaignProgression.SelectMission;
        _campaignProgression.MissionSelected += _missionUI.ShowMissionPreview;

        _missionUI.MissionStarted += _campaignProgression.StartSelectedMission;
        _campaignProgression.MissionStarted += _ => _inputsService.DisableInputs();
        _campaignProgression.MissionStarted += _missionUI.ShowMissionAccomplishment;
        _campaignProgression.MissionStarted += _ => _heroesUI.DisableSelecting();

        _missionUI.MissionCompleted += _campaignProgression.CompleteStartedMission;
        _heroesUI.HeroSelected += _campaignProgression.SelectHero;
        _campaignProgression.MissionCompleted += (_, __) => _missionUI.HideMission();
        _campaignProgression.MissionCompleted += (_, __) => _heroesUI.EnableSelecting();
        _heroesStorage.HeroAdded += _heroesUI.AddHero;
        _heroesStorage.HeroDataUpdated += _heroesUI.SetHeroData;
        _campaignProgression.MissionCompleted += (_, __) => _inputsService.EnableInputs();

        _inputsService.Initialize();

        _heroesUI.Initialize(_heroesStorage);

        _map.Initialize(_missionsStorage);
    }
}