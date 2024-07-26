using UnityEngine;
using System.Collections;

public class UICameraHandler : MonoBehaviour
{
    void OnEnable ()
    {
        SetCameraSize();
    }

	void Start ()
    {
        SetCameraSize();
	}

    void SetCameraSize ()
    {
        GetComponent<Camera>().orthographicSize = Camera.main.orthographicSize;
        Debug.Log("Camera size set to: " + GetComponent<Camera>().orthographicSize);
    }
}
