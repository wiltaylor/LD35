using System.Collections.Generic;
using UnityEngine;

public class PlayerPersistData : MonoBehaviour
{
    public int Gold;
    public bool GamePaused;
    public int SpeedRank;
    public int DamageRank;
    public int HPRank;
    public float SpeedModifier;
    public float DamageModifier;
    public float HPModifier;
    public float HPRechargeRate;
    public bool DirtyData;
    public int LowestLevelVisited;
    public List<string> CompletedChatBoxes = new List<string>();
}
