using System;
using UnityEngine;
using System.Collections;

public class MeleeBeacon : MonoBehaviour
{

    public WalkingAI AI;

    public void OnTriggerStay2D(Collider2D other)
    {
        AI.OnTriggerStay2D(other);
    }
}
