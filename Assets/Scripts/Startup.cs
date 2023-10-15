using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private MissionsContainer _missionsContainer;
    
    void Awake()
    {
        
    }
}

[CreateAssetMenu(menuName = "Missions", fileName = "MissionsContainerAsset", order = 1)]
public class MissionsContainer : ScriptableObject
{
    public List<IMission> Missions;
    private int _engkrnge;

    public void UpdateMissionsState()
    {
        UpdateMissionsState
    }
}

public class Mission
{
    private List<MissionConfig> requirement; // список миссий, которые мы должны пройти, чтобы разблокировать текущую
    private List<MissionConfig> missionsToShow; //список миссий, которые мы отобразим после того, как эта миссия разблокируется
}