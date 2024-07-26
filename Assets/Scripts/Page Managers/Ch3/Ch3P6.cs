using UnityEngine;
using System.Collections;

public class Ch3P6 : MonoBehaviour {
	public GameObject Panel1;
	public GameObject Panel2;
	public GameObject goatMan;
	int mouseCount=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)&&mouseCount==0) {
			Panel1.SetActive(true);
			mouseCount = 1;
		}
		else if (Input.GetMouseButtonUp (0)&&mouseCount==1) {
			Panel2.SetActive(true);
			mouseCount = 2;
		}
		else if (Input.GetMouseButtonUp (0)&&mouseCount==2) {
			goatMan.SetActive(true);
			mouseCount = 3;
		}
	}
}
