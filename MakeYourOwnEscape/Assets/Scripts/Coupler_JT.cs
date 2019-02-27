using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coupler_JT : JoinedTransform
{
    [SerializeField]
    private Card parent_card;

    protected override void OnTriggerEnter(Collider col)
    {
        Vector3 translate = col.transform.position - transform.position;

        parent_card.ForcePos(transform.position + translate);
    }
    
}
