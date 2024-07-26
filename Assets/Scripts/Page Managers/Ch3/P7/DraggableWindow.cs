using UnityEngine;
using System.Collections;
using Fungus;

public class DraggableWindow : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    
    [SerializeField]
    private Utils.clamp xyClamp;

    //private new Animation animation;
    void Start()
    {
        //animation = gameObject.GetComponent<Animation>();
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        //if (!animation.IsPlaying("Window_breathing"))
        {
            //animation.Play();
        }  
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void Update()
    {
        //The following lines of code,clamp the window inside the viewing space
        Vector3 currentpos = transform.position;
        currentpos.x = Mathf.Clamp(currentpos.x, xyClamp.xMin, xyClamp.xMax);
        currentpos.y = Mathf.Clamp(currentpos.y, xyClamp.yMin, xyClamp.yMax);
        transform.position = currentpos;


       
    }



    /*  code to drag the object using Physics
    public float distanceFromCamera;
    public Camera cam;
    Rigidbody2D r;


    void Start()
    {
        distanceFromCamera = Vector3.Distance(transform.position, cam.transform.position);
        r = GetComponent<Rigidbody2D>();
    }

    Vector3 lastPos;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            pos.z = distanceFromCamera;
            pos = cam.ScreenToWorldPoint(pos);
            r.velocity = (pos - transform.position)*2;
        }
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "prop_powerCube")
        {
            Debug.Log("Collider hit !! ");
        }
    }
    */
}

