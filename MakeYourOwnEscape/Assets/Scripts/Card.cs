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

    [SerializeField]
    private GameObject imageLock, imageUnlock;

    public float joint_force;
#endregion

#region Fields
	int _nb_elmnt_inside;

    bool isForcedPos = false, isForceRot = false;

    Collider _ownCollider;

    Vector3 offset = Vector3.up *3;
    GameObject imageLockUsed;

    private int actualLevel = 0;

    Vector3 focusedPos, focusedRot;

    float offsetWith = 0.005f, worldScale;
#endregion

#region Unity Methods
	// Use this for initialization
	void Start () {
        focusedPos = originPos;
        focusedRot = originRot;
        
        //_ownCollider = GetComponent<Collider>();
	}

    void OnEnable()
    {
        worldScale = GameManager.GetWorldScaleFromTransform(transform);
        Debug.Log("Set world Scale : " + worldScale);
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
            /*if (isForceRot)
            {
                transform.eulerAngles = focusedRot;
            }*/
            Vector3 localWithoutHeight = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
            //Debug.Log("new Pos : " + transform.position + " | Local Pos : " + transform.localPosition + " | sqrt Mag : " + localWithoutHeight.sqrMagnitude);
            if (localWithoutHeight.sqrMagnitude > joint_force)
            {
                transform.localPosition = originPos;
                transform.localEulerAngles = originRot;
                focusedPos = originPos;
                focusedRot = originRot;
                isForcedPos = false;
                isForceRot = false;
                //changeLock(isForceRot);
                //imageLockUsed.SetActive(false);
                virtualButton.SetActive(false);
                Debug.Log("No longer forcing pose !");
            }

            //setImagePos();


        }else
        {
            /*
            Vector3 target = getPosBlock();
            if (target != Vector3.zero)
            {
                Debug.Log("found pose : " + target);
                focusedPos = target;
                transform.position = focusedPos;
                isForcedPos = true;
            }*/
        }
    }

    void onTriggerEnter(Collider col)
    {
        _nb_elmnt_inside++;
        forbiddenBuildCube.SetActive(true);
    }

    void onTriggerExist(Collider col)
    {
        _nb_elmnt_inside--;
        if (_nb_elmnt_inside <= 0)
        {
            forbiddenBuildCube.SetActive(false);
        }
    }

    #endregion

    #region Methods

    public void GoUp()
    {
        if(actualLevel < GameManager.instance.GetActualMaxLevelHeight())
        {
            actualLevel++;
            float height_step = GameManager.instance.GetHeightStep();
            transform.localPosition = originPos + Vector3.up * ((height_step * transform.localScale.x) * actualLevel);
        }
    }

    public void GoDown()
    {
        if(actualLevel > 0)
        {
            actualLevel--;
            float height_step = GameManager.instance.GetHeightStep();
            transform.localPosition = originPos + Vector3.up * ((height_step * transform.localScale.x) * actualLevel);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        throw new NotImplementedException();
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

    public void ForcePos(Vector3 newFocusedPos, Vector3 newFocusedRot)
    {
        if (focusedPos != originPos)
        {
            if (newFocusedPos.sqrMagnitude < focusedPos.sqrMagnitude)
            {
                focusedPos = newFocusedPos;
                focusedRot = newFocusedRot;
            }
        }
        else
        {
            isForcedPos = true;
            isForceRot = true;
            //changeLock(isForceRot);
            //imageLockUsed.SetActive(true);
            if (virtualButton != null)
            {
                virtualButton.SetActive(true);
            }
            focusedPos = newFocusedPos;
            focusedRot = newFocusedRot;
        }


    }

    #endregion

    #region Private Functions


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

    void setImagePos()
    {
        Vector3 posUI = Camera.main.WorldToScreenPoint(transform.position);
        posUI += offset;
        // imageLock.transform.position = posUI;
        //  imageUnlock.transform.position = posUI;
        imageLockUsed.transform.position = posUI;

    }

    void changeLock(bool forcedLock)
    {
        imageLockUsed = forcedLock ? imageLock : imageUnlock;
        imageLock.SetActive(forcedLock);
        imageUnlock.SetActive(!forcedLock);
    }

    Vector3 getPosBlock()
    {
        Vector3 posFinal = Vector3.zero;

        int layerMask = 1 << 13; // numero du layer blockScene

        var minPos = Mathf.Infinity;
        Transform bestBlockTransform = null;

        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 0.0001f, transform.forward,  worldScale * 1.7f, layerMask);
       
        for(int i=0; i<hit.Length; ++i)
        {
            
            Debug.Log("found collider : " + hit[i].collider.name);
            var sqrmagnitude = (hit[i].transform.position - transform.position).sqrMagnitude;
            if (sqrmagnitude < minPos)
            {
                minPos = sqrmagnitude;
                bestBlockTransform = hit[i].transform;
            }         
        }

        if(bestBlockTransform != null)
        {
            var dir = bestBlockTransform.position - transform.position;
            if(Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
            {
                posFinal = new Vector3(bestBlockTransform.position.x - Mathf.Sign(dir.x) * offsetWith, bestBlockTransform.position.y, bestBlockTransform.position.z);
            }
            else
            {
                posFinal = new Vector3(bestBlockTransform.position.x, bestBlockTransform.position.y, bestBlockTransform.position.z - Mathf.Sign(dir.z) * offsetWith);
            }
        }
        
        return posFinal;
    }
    #endregion
}
