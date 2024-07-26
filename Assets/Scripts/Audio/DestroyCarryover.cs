using UnityEngine;
using System.Collections;

public class DestroyCarryover : MonoBehaviour
{
	void Start ()
    {
        CarryoverSFX c = FindObjectOfType<CarryoverSFX>();
        if (c)
            Destroy(FindObjectOfType<CarryoverSFX>().gameObject);
        Destroy(GameObject.Find("Carryover SFX"));
    }
	
}
