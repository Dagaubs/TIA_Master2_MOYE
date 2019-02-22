using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Static

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private GameObject panelMobileControl, panelLevelEditing;

    [SerializeField]
    private GameObject _startTower;
#endregion

#region Script Parameters
#endregion

#region Fields
#endregion

#region Unity Methods
	// Use this for initialization
	void Start () {
        panelMobileControl.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	}
#endregion

#region Methods

    public void FinishLevelEditing()
    {

    }
#endregion

#region Private Functions
#endregion
}
