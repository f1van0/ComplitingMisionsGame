using System;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardUI : MonoBehaviour
{
    public event Action StateChanged;

    [SerializeField] private Toggle _toggle;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _scoreLabel;
    [SerializeField] private Outline _outline;

    [HideInInspector] public HeroType HeroType;

    public void Start()
    {
        _toggle.onValueChanged.AddListener(ChangeState);
    }

    public void Setup(HeroData heroData, ToggleGroup toggleGroup)
    {
        _toggle.group = toggleGroup;
        SetValues(heroData);
    }

    public void SetValues(HeroData heroData)
    {
        _nameLabel.text = heroData.Type.ToString();
        _scoreLabel.text = heroData.Score.ToString();
        HeroType = heroData.Type;
    }

    public void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(ChangeState);
    }
    
    private void ChangeState(bool state)
    {
        if (state)
            _outline.effectColor = Color.green;
        else
            _outline.effectColor = Color.black;

        StateChanged?.Invoke();
    }

    public void SetInteractivity(bool state)
    {
        _toggle.interactable = state;
    }
}