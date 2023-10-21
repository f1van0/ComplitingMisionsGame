using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/MissionConfig", fileName = "MissionConfigAsset", order = 3)]
public class MissionConfigSO : ScriptableObject
{
    public Guid Id = Guid.NewGuid();

    public string Number;
    public string Name;
    public string Foreword;
    public string Description;
    public string PlayerSide;
    public string EnemySide;

    [Space]
    public Vector2 Position;
    
    [Space]
    public List<HeroType> UnlockingHeroes;
    public List<InspectorKeyValue<HeroType, int>> HeroPoints;
}