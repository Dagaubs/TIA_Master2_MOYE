using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Static

    private static GameManager instance;

    #endregion

    #region Script Parameters
    private GameObject _actualSetPositionPanel = null;
    #endregion

    #region Fields

    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private GameObject panelMobileControl, panelLevelEditing, player_prefab;

    [SerializeField]
    private Transform _spawnPoint;

    private Player3D actualPlayer;
    #endregion

    #region Unity Methods
    // Use this for initialization
    void Start () {
        instance = this;
        panelMobileControl.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	}
#endregion

#region Methods

    public void FinishLevelEditing()
    {
        // Modify UI
        panelMobileControl.SetActive(true);
        panelLevelEditing.SetActive(false);

        //Spawn Player
        spawnPlayer();

    }

    private void spawnPlayer()
    {
        actualPlayer = Instantiate(player_prefab, _spawnPoint.position, _spawnPoint.rotation).GetComponent<Player3D>();
        actualPlayer.SpawnPlayer();
    }

    public bool DisplaySetPositionUI(SetPositionHandler caller)
    {
        if (_actualSetPositionPanel == null)
        {

            return true;
        }
        else return false;
    }
#endregion

#region Private Functions
#endregion
}
