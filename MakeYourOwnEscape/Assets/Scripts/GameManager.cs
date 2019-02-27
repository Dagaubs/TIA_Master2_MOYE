using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Static

    private static GameManager instance;

    #endregion

    #region Script Parameters
    private GameObject _actualSetPositionPanel = null;
    #endregion

    #region Fields

    [SerializeField]
    private int nbLife;

    [SerializeField]
    private TMPro.TextMeshProUGUI nbLifeText;

    [SerializeField]
    private Canvas GameOverCanvas;

    [SerializeField]
    private Player3D actualPlayer;
    #endregion

    #region Unity Methods
    // Use this for initialization
    void Start () {
        instance = this;
        nbLifeText.text = "x" + nbLife;
    }
	
	// Update is called once per frame
	void Update () {
	}
#endregion

#region Methods
    public void spawnPlayer()
    {
        //actualPlayer = Instantiate(player_prefab, _spawnPoint.position, _spawnPoint.rotation).GetComponent<Player3D>();
    //    actualPlayer.SpawnPlayer(); A REMETTRE QUAND IL SERA DANS LA SCENE !!
    }

    public bool DisplaySetPositionUI(SetPositionHandler caller)
    {
        if (_actualSetPositionPanel == null)
        {

            return true;
        }
        else return false;
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Die()
    {
        nbLife--;
        nbLifeText.text = "x" + nbLife;
        if(nbLife <= 0)
        {
            GameOverCanvas.enabled = true;
        }
    }
    #endregion

    #region Private Functions
    #endregion
}
