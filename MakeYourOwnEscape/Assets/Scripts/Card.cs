using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Card : MonoBehaviour, IVirtualButtonEventHandler
{

#region Static
#endregion

#region Script Parameters
	public GameObject forbiddenBuildCube;

    public Vector3 originPos, originRot;

    [SerializeField]
    private GameObject virtualButton;

    public float joint_force;
#endregion

#region Fields
	int _nb_elmnt_inside;

    bool isForcedPos = false, isForceRot = false;

    Vector3 focusedPos, focusedRot;

#endregion

#region Unity Methods
	// Use this for initialization
	void Start () {
        focusedPos = originPos;
        focusedRot = originRot;
	}

    public void ForcePos(Vector3 newFocusedPos, Vector3 newFocusedRot)
    {
        if(focusedPos != originPos)
        {
            if(newFocusedPos.sqrMagnitude < focusedPos.sqrMagnitude)
            {
                focusedPos = newFocusedPos;
                focusedRot = newFocusedRot;
            }
        }
        else
        {
            isForcedPos = true;
            isForceRot = true;
            if(virtualButton != null)
            {
                virtualButton.SetActive(true);
            }
            focusedPos = newFocusedPos;
            focusedRot = newFocusedRot;
        }


    }
    	
	// Update is called once per frame
	void Update () {
        if (isForcedPos)
        {
            Debug.Log("Card is forced ");
            if(transform.position != focusedPos)
            {
                Debug.Log("actual pos : " + transform.position + " | target : " + focusedPos);
            }
            transform.position = focusedPos;
            if (isForceRot)
            {
                transform.eulerAngles = focusedRot;
            }
            Vector3 localWithoutHeight = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            Debug.Log("new Pos : " + transform.position + " | Local Pos : " + transform.localPosition + " | sqrt Mag : " + localWithoutHeight.sqrMagnitude);
            if (localWithoutHeight.sqrMagnitude > joint_force)
            {
                transform.localPosition = originPos;
                transform.localEulerAngles = originRot;
                focusedPos = originPos;
                focusedRot = originRot;
                isForcedPos = false;
                isForceRot = false;
                virtualButton.SetActive(false);
                Debug.Log("No longer forcing pose !");
            }
        }
	}

	void onTriggerEnter(Collider col){
		_nb_elmnt_inside++;
		forbiddenBuildCube.SetActive(true);
	}

	void onTriggerExist(Collider col){
		_nb_elmnt_inside--;
		if(_nb_elmnt_inside <= 0){
			forbiddenBuildCube.SetActive(false);
		}
	}

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        throw new NotImplementedException();
    }

    private void switchForceRotState()
    {
        if (!isForcedPos)
        {
            Debug.LogError("Shouldn't be able to switch rotState cause not forced POs !");
        }
        else
        {
            isForceRot = !isForceRot;
            if (isForceRot)
            {
                focusedRot = transform.eulerAngles;
            }
            // TODO: set UI
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        if (vb.VirtualButtonName == "LockButton")
        {
            if (!isForcedPos)
            {
                Debug.LogError("Shouldn't be able to hit button !");
            }
            else
            {
                switchForceRotState();
            }

        }
    }
    #endregion

    #region Methods
    #endregion

    #region Private Functions
    #endregion
}
