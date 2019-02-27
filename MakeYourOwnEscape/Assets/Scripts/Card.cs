using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

#region Static
#endregion

#region Script Parameters
	public GameObject forbiddenBuildCube;

    public Vector3 originPos;

    public float joint_force;
#endregion

#region Fields
	int _nb_elmnt_inside;

    bool isForcedPos = false;

    Vector3 focusedPos;

#endregion

#region Unity Methods
	// Use this for initialization
	void Start () {
        focusedPos = originPos;
	}

    public void ForcePos(Vector3 newFocusedPos)
    {
        if(focusedPos != originPos)
        {
            if(newFocusedPos.sqrMagnitude < focusedPos.sqrMagnitude)
            {
                focusedPos = newFocusedPos;
            }
        }
        else
        {
            isForcedPos = true;
            focusedPos = newFocusedPos;
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
            Vector3 localWithoutHeight = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            Debug.Log("Local Pos : " + transform.localPosition + " | sqrt Mag : " + localWithoutHeight.sqrMagnitude);
            if (localWithoutHeight.sqrMagnitude > joint_force)
            {
                transform.localPosition = originPos;
                focusedPos = originPos;
                isForcedPos = false;
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
