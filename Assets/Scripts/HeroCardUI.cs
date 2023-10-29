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

    public void Setup(Hero hero, ToggleGroup toggleGroup)
    {
        _toggle.group = toggleGroup;
        SetValues(hero);
    }

    public void SetValues(Hero hero)
    {
        _nameLabel.text = hero.Type.ToString();
        _scoreLabel.text = hero.Score.ToString();
        HeroType = hero.Type;
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