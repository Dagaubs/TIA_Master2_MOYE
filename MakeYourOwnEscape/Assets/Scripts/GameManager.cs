using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Static

    public static GameManager instance;

    #endregion

    #region Script Parameters
    private GameObject _actualSetPositionPanel = null;

    public static float GetWorldScaleFromTransform(Transform trans)
    {
        Transform actual = trans;
        float ret = trans.localScale.x;
        while (actual.parent != null)
        {
            actual = actual.parent;
            ret *= actual.localScale.x;
        }
        return ret;
    }

    #endregion

    #region Fields

    [SerializeField]
    private int nbLife, nbLevelOfHeight;

    [SerializeField]
    private float height_step;

    [SerializeField]
    private TMPro.TextMeshProUGUI nbLifeText;

    [SerializeField]
    private TMPro.TextMeshProUGUI dialogText;

    [SerializeField]
    private Transform _spawnPoint;

    [SerializeField]
    private Canvas GameOverCanvas, DialogCanvas;

    [SerializeField]
    private GameObject player_prefab;

    private Player3D actualPlayer;
    
    #endregion

    #region Unity Methods
    // Use this for initialization
    void Start () {
        instance = this;
        nbLifeText.text = "x" + nbLife;
        dialogText.text = "c'est parti pour l'aventure"; //change for the real text
    }
	
	// Update is called once per frame
	void Update () {
	}
#endregion

#region Methods
    public int GetActualMaxLevelHeight()
    {
        return nbLevelOfHeight;
    }

    public float GetHeightStep()
    {
        return height_step;
    }

    public void spawnPlayer()
    {
        actualPlayer = Instantiate(player_prefab, _spawnPoint.position, _spawnPoint.rotation, _spawnPoint.parent).GetComponent<Player3D>();
        Debug.Log("Spawn player to world pos : " + _spawnPoint.position);
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
        else
        {
            spawnPlayer();
        }
    }

    public void winGame()
    {
        dialogText.text = "win";
        DialogCanvas.enabled = true;
        
    }
    #endregion

    #region Private Functions
    #endregion
}
