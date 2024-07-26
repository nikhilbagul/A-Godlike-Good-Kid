using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClockHandMask : MonoBehaviour {

    public GameObject objectToFollow;
    private RectTransform handMask;

	// Use this for initialization
	void Start ()
    {
        handMask = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        handMask.rotation = Quaternion.Euler(new Vector3(0, 0, objectToFollow.transform.rotation.eulerAngles.z));
	}
}
