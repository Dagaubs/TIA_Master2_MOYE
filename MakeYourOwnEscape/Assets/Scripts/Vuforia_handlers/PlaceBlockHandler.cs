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

    void Start()
    {
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for(int i = 0; i < vbs.Length; i++)
        {
            vbs[i].RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("button pressed : " + vb.VirtualButtonName);
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("released");
        if(vb.VirtualButtonName == "PlaceBlock")
        {
            if (placed)
            {
                child_obj.transform.SetParent(IT_transform, false);
                placed = false;
            }
            else
            {
                Debug.Log("child_obj active ? " + child_obj.activeSelf);
                child_obj.transform.SetParent(placed_blocks_transform, false);
                placed = true;
            }
        }
    }
}
