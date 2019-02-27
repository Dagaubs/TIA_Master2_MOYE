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

        Debug.Log(gameObject.name + " triggered with " + col.name + " | moving gameobject to " + parent_card.transform.position + translate);
        
        parent_card.ForcePos(parent_card.transform.position + translate);
    }
    
}
