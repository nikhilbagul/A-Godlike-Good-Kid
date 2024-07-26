using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class clockMask_handler : MonoBehaviour
{

    public ClockHand_rotatorCCW CCW_object;
    public ClockHand_rotatorCW CW_objects;
    public GameObject CCW_hand, CW_hand;
    public Image fill_image;

	// Use this for initialization
	void Start ()
    {
        fill_image = gameObject.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (CCW_object.counter_clockwise)
        {
            fill_image.fillAmount = 1.0f - (CCW_hand.transform.rotation.eulerAngles.z / 360.0f);
        }
	}
}
