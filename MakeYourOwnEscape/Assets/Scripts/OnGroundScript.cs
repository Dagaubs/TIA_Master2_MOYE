using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundScript : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            //restart
            Debug.Log("restart");
        }
    }
}
