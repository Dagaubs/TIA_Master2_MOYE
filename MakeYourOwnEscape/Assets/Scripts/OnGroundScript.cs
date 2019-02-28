using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //restart
            Debug.Log("Felll of the GORuuuurogue");
            GameManager.instance.Die();
            Destroy(collider.gameObject);
        }
    }
}
