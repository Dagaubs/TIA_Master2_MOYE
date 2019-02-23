using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlaceBlockHandler : MonoBehaviour, IVirtualButtonEventHandler
{

    [SerializeField]
    private Transform IT_transform, placed_blocks_transform;
    [SerializeField]
    private GameObject child_obj;

    bool placed = false;


    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("button pressed : " + vb.VirtualButtonName);
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        if(vb.VirtualButtonName == "PlaceBlock")
        {
            if (placed)
            {
                child_obj.transform.SetParent(IT_transform, false);
                placed = false;
            }
            else
            {
                child_obj.transform.SetParent(placed_blocks_transform, false);
                placed = true;
            }
        }
    }
}
