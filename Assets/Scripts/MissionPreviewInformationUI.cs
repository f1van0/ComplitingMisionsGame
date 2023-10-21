﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MissionPreviewInformationUI : MonoBehaviour
{
    public event Action<Guid> StartMissionPressed;
    
    [SerializeField] private Button _startButton;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _forewordLabel;

    private Guid _id;

    public void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonPressed);
    }
    
    public void Setup(MissionConfigSO config)
    {
        _nameLabel.text = config.Name;
        _forewordLabel.text = config.Foreword;
    }

    private void OnStartButtonPressed()
    {
        StartMissionPressed?.Invoke(_id);
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
    }
}