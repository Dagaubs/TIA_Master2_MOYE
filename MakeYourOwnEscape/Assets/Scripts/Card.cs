using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

#region Static
#endregion

#region Script Parameters
	public GameObject forbiddenBuildCube;

    public Vector3 originPos, originRot;

    public float joint_force;
#endregion

#region Fields
	int _nb_elmnt_inside;

    bool isForcedPos = false;

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
            transform.eulerAngles = focusedRot;
            Vector3 localWithoutHeight = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            Debug.Log("new Pos : " + transform.position + " | Local Pos : " + transform.localPosition + " | sqrt Mag : " + localWithoutHeight.sqrMagnitude);
            if (localWithoutHeight.sqrMagnitude > joint_force)
            {
                transform.localPosition = originPos;
                transform.localEulerAngles = originRot;
                focusedPos = originPos;
                focusedRot = originRot;
                isForcedPos = false;
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
#endregion

#region Methods
#endregion

#region Private Functions
#endregion
}
