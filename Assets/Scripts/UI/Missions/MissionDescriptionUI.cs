using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Missions
{
    public class MissionDescriptionUI : MonoBehaviour
    {
        public event Action MissionCompleted;
    
        [SerializeField] private TMP_Text _nameLabel;
        [SerializeField] private TMP_Text _playerSideLabel;
        [SerializeField] private TMP_Text _enemySideLabel;
        [SerializeField] private TMP_Text _descriptionLabel;
        [SerializeField] private Button _completeMissionButton;

        private MissionConfigSO _missionConfig;

        private void Awake()
        {
            _completeMissionButton.onClick.AddListener(CompleteMission);
        }

        private void CompleteMission()
        {
            MissionCompleted?.Invoke();
        }

        public void Setup(MissionConfigSO config)
        {
            _missionConfig = config;
            _nameLabel.text = config.Name;
            _playerSideLabel.text = config.PlayerSide;
            _enemySideLabel.text = config.EnemySide;
            _descriptionLabel.text = config.Description;
        }

        private void OnDestroy()
        {
            _completeMissionButton.onClick.RemoveAllListeners();
        }
    }
}