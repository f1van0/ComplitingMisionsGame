using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/MissionDataAsset", fileName = "MissionDataAsset", order = 1)]
public abstract class MissionConfig : ScriptableObject
{
    public Guid Id = Guid.NewGuid();

    public string Name;
    public string Foreword;
    public string Description;
    public string PlayerSide;
    public string EnemySide;
    
    public Vector2 Position;
}