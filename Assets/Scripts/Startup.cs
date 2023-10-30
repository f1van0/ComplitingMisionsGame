using System;
using System.Collections;
using Data.Heroes;
using Data.Missions;
using Game;
using Game.Map;
using ScriptableObjects;
using Services;
using UI.Heroes;
using UI.Missions;
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

        SubscribeOnCampaignProgression();
        SubscribeOnMap();
        SubscribeOnMissionUI();
        SubscribeOnHeroesUI();
        SubscribeOnHeroesStorage();

        _inputsService.Initialize();
        _heroesUI.Initialize(_heroesStorage);
        _map.Initialize(_missionsStorage);
    }

    private void SubscribeOnCampaignProgression()
    {
        _campaignProgression.MissionSelected += _missionUI.ShowMissionPreview;
        _campaignProgression.MissionStarted += _missionUI.ShowMissionAccomplishment;
        _campaignProgression.MissionCompleted += (_, __) => _missionUI.HideMission();
        
        _campaignProgression.MissionStarted += _ => _inputsService.DisableInputs();
        _campaignProgression.MissionCompleted += (_, __) => _inputsService.EnableInputs();
        
        _campaignProgression.MissionStarted += _ => _heroesUI.DisableSelecting();
        _campaignProgression.MissionCompleted += (_, __) => _heroesUI.EnableSelecting();
    }

    public void SubscribeOnMap()
    {
        _map.SelectedMission += _campaignProgression.SelectMission;
    }

    private void SubscribeOnMissionUI()
    {
        _missionUI.MissionStarted += _campaignProgression.StartSelectedMission;
        _missionUI.MissionCompleted += _campaignProgression.CompleteStartedMission;
    }

    private void SubscribeOnHeroesUI()
    {
        _heroesUI.HeroSelected += _campaignProgression.SelectHero;
    }

    private void SubscribeOnHeroesStorage()
    {
        _heroesStorage.HeroAdded += _heroesUI.AddHero;
        _heroesStorage.HeroDataUpdated += _heroesUI.SetHeroData;
    }
}