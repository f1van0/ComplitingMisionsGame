using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission", fileName = "MissionDataAsset", order = 1)]
public abstract class MissionData : ScriptableObject
{
    public string Name;
    public string Foreword;
    public string Description;
    public string PlayerSide;
    public string EnemySide;

    public Guid Id = Guid.NewGuid();
    
    public Vector2 Position;
    
    public MissionState MissionState;
}