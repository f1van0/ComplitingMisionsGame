using System;
using TMPro;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private MissionDescriptionUI _missionDescriptionUI;
    
    private CampaignProgression _campaignProgression;
    private Map _map;

    public void Initialize(Map map, CampaignProgression campaignProgression)
    {
        _map = map;
        _campaignProgression = campaignProgression;
        _map.SelectedMission += ShowMission;
    }

    private void ShowMission(Guid missionId)
    {
        _missionDescriptionUI.Setup(_campaignProgression.GetMissionConfig(missionId));
    }

    private void OnDestroy()
    {
        _map.SelectedMission -= ShowMission;
    }
}

public class MissionDescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _forewordLabel;
    [SerializeField] private TMP_Text _playerSideLabel;
    [SerializeField] private TMP_Text _enemySideLabel;
}