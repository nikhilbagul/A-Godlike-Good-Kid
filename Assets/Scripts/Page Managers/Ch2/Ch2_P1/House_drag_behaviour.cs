using UnityEngine;
using System.Collections;

public class House_drag_behaviour : MonoBehaviour {

    public SpriteRenderer largeHouse, draggableHouse, draggableHouse_glow;
    private float housescale;
    // Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameObject.transform.position.y <= 4.0f && gameObject.transform.position.y >= 0.0f)
        {
            largeHouse.color = new Color(largeHouse.color.r, largeHouse.color.g, largeHouse.color.b,  (4.0f - gameObject.transform.position.y)/4.0f);
            draggableHouse.color = new Color(draggableHouse.color.r, draggableHouse.color.g, draggableHouse.color.b, 1.0f - ((4.0f - gameObject.transform.position.y) / 4.0f));
            draggableHouse_glow.color = new Color(draggableHouse_glow.color.r, draggableHouse_glow.color.g, draggableHouse_glow.color.b, 1.0f - ((4.0f - gameObject.transform.position.y) / 4.0f));

            housescale = (40.0f - 7 * gameObject.transform.position.y) / 80.0f;
            //print(housescale); 
            largeHouse.gameObject.transform.localScale = new Vector3(housescale, housescale, 0.0f);
        }

        if(gameObject.transform.position.y > 4.0f)
        {
            largeHouse.color = new Color(largeHouse.color.r, largeHouse.color.g, largeHouse.color.b, 0.0f);
            draggableHouse.color = new Color(largeHouse.color.r, largeHouse.color.g, largeHouse.color.b, 1.0f);            
        }

        if (gameObject.transform.position.y < 0.0f)
        {
            largeHouse.color = new Color(largeHouse.color.r, largeHouse.color.g, largeHouse.color.b, 1.0f);
            draggableHouse.color = new Color(largeHouse.color.r, largeHouse.color.g, largeHouse.color.b, 0.0f);
            draggableHouse_glow.color = new Color(draggableHouse_glow.color.r, draggableHouse_glow.color.g, draggableHouse_glow.color.b, 0.0f);
        }
    }


}
