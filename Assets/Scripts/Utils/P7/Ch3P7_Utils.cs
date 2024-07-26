using UnityEngine;
using System.Collections;
using Fungus;
using System.Collections.Generic;

public class Ch3P7_Utils : MonoBehaviour {

    public GameObject gameobject_collider;
    
    // Use this for initialization
    void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void disableCollider()
    {
        gameobject_collider.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void enableCollider()
    {
        gameobject_collider.GetComponent<BoxCollider2D>().enabled = true;
    }
}
