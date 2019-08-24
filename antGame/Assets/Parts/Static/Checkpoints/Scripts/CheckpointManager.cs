using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint[] checkpoints;

    public int currentCheckpoint;

    public void UpdateCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i].active == true)
            {
                currentCheckpoint = i;
            }
        }
    }
}
