using System.Collections;
using UnityEngine;

public class Startup : MonoBehaviour
{
    [SerializeField] private MissionsContainerSO _missions;
    [SerializeField] private Map _map;
    
    private CampaignProgression _campaignProgression;

    void Awake()
    {
        _campaignProgression = new CampaignProgression(_missions);
        _map.Initialize(_campaignProgression.MissionsContainer);
    }
}