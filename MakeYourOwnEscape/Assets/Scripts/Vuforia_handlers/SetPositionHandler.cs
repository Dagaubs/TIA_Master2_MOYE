using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionHandler : DefaultTrackableEventHandler
{
    private bool alreadyPlaced = false, waitingForLostTrack = false;
    public bool isAlreadyPlaced() { return alreadyPlaced; }

    private Transform savedTransform;

    protected override void OnTrackingFound()
    {
        if (!alreadyPlaced)
        {
            base.OnTrackingFound();
        }
        else
        {

        }
    }

    protected override void OnTrackingLost()
    {
        if (waitingForLostTrack)
        {
            transform.position = savedTransform.position;
            transform.rotation = savedTransform.rotation;
        }
        else
        {
            base.OnTrackingLost();
        }
    }
}
