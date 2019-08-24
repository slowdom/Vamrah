using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;

    public bool active;

    private void Start()
    {
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            active = true;
            
            checkpointManager.UpdateCheckpoints();
        }
    }
}
