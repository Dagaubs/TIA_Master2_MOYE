using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ButtonUpHandler : MonoBehaviour, IVirtualButtonEventHandler
{
    [SerializeField]
    private Card card;/*
    [SerializeField]
    private string StringName;*/

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log(vb.VirtualButtonName + " was pressed !");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        if (vb.VirtualButtonName == "UP_Boxes")
        {
            Debug.Log(card.name + "  is requested UP");
            card.GoUp();
        }
    }
}
