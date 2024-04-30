using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureReward : MonoBehaviour
{
    [SerializeField]
    private bool isRewardable;
    [SerializeField]
    private int endOfRoundReward;
    

    public int EndOfRoundReward
    {
        get => endOfRoundReward;
        set
        {
            if (isRewardable)
            {
                endOfRoundReward = value;
  
                Debug.Log($"End of round reward updated to: {endOfRoundReward}");
            }
            else
            {
                Debug.LogWarning("Attempted to set end of round reward but the structure is not rewardable.");
            }
        }
    }


    public bool IsRewardable
    {
        get => isRewardable;
        set => isRewardable = value;
    }
}
