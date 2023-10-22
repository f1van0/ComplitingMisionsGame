using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MissionPoint : SelectableGameObject
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _numberText;
    
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _lockedColor;
    [SerializeField] private Color _completedColor;
    [SerializeField] private Color _unavailableColor;

    public Guid Id;

    public void Setup(MissionConfigSO config, MissionState state)
    {
        Id = config.Id;
        _numberText.text = config.Number;
        SetState(state);
    }

    public void SetState(MissionState state)
    {
        gameObject.SetActive(state != MissionState.Unavailable);
        _spriteRenderer.color = GetColorForState(state);
    }

    private Color GetColorForState(MissionState state)
    {
        switch (state)
        {
            case MissionState.Active:
                return _activeColor;
            case MissionState.Completed:
                return _completedColor;
            case MissionState.Locked:
                return _lockedColor;
            default:
                return _unavailableColor;
        }
    }
}