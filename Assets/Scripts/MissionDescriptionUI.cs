using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDescriptionUI : MonoBehaviour
{
    public Action MissionCompleted;
    
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _playerSideLabel;
    [SerializeField] private TMP_Text _enemySideLabel;
    [SerializeField] private TMP_Text _descriptionLabel;
    [SerializeField] private Button _completeMissionButton;

    private MissionConfigSO _missionConfig;

    public void Setup(MissionConfigSO config)
    {
        _missionConfig = config;
        _nameLabel.text = config.name;
        _playerSideLabel.text = config.PlayerSide;
        _enemySideLabel.text = config.EnemySide;
        _descriptionLabel.text = config.Description;
    }
}