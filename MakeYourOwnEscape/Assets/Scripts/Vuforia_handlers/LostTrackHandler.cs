using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostTrackHandler : DefaultTrackableEventHandler
{
    Vector3 savedPosition;

    protected override void OnTrackingLost()
    {
        savedPosition = transform.position;
    }
}
