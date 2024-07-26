using UnityEngine;
using System.Collections;

public class Ch3P1_writingTable : MonoBehaviour {
	public GameObject textOnWritingTable;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			textOnWritingTable.SetActive (true);
			gameObject.SetActive (false);
		}
	}
}
