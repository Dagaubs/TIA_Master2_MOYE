using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

#region Static
#endregion

#region Script Parameters
	public GameObject forbiddenBuildCube;
#endregion

#region Fields
	int _nb_elmnt_inside;
#endregion

#region Unity Methods
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
