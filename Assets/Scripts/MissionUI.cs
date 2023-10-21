using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MissionUI : MonoBehaviour
{
    public event Action<Guid> MissionStarted;
    public event Action<Guid> MissionCompleted;
    
    [SerializeField] private MissionPreviewInformationUI _mainMissionPreviewUI;
    [SerializeField] private MissionPreviewInformationUI _additionalMissionPreviewUI;
    [SerializeField] private MissionDescriptionUI _missionDescriptionUI;

    

    public void ShowMissionPreview(MissionDefinition definition, Guid id)
    {
        switch (definition)
        {
            case SingleMissionDefinition single:
                _mainMissionPreviewUI.Setup(single.Mission.Config);
                _mainMissionPreviewUI.gameObject.SetActive(true);
                _additionalMissionPreviewUI.gameObject.SetActive(false);
                break;
            case DualMissionDefinition dual:
                _mainMissionPreviewUI.Setup(dual.Mission1.Config);
                _mainMissionPreviewUI.Setup(dual.Mission2.Config);
                _mainMissionPreviewUI.gameObject.SetActive(true);
                _additionalMissionPreviewUI.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(definition));
        }
    }

    public void HideMission()
    {
        _mainMissionPreviewUI.gameObject.SetActive(false);
        _additionalMissionPreviewUI.gameObject.SetActive(false);
        _missionDescriptionUI.gameObject.SetActive(false);
    }

    public void ShowMissionAccomplishment(MissionConfigSO config)
    {
        _mainMissionPreviewUI.gameObject.SetActive(false);
        _additionalMissionPreviewUI.gameObject.SetActive(false);
        _missionDescriptionUI.gameObject.SetActive(true);
        _missionDescriptionUI.Setup(config);
    }
}