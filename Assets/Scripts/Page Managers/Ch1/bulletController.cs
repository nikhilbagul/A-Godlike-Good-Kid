using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Fungus;

public class bulletController : MonoBehaviour {
    public Flowchart flowchart;

    Vector2 origin, upperDirection, botDirection;
    public GameObject originPoint, upperPoint, botPoint;
    public Slider progressSlider;
    const float turnRatio = 360f;
    Quaternion rotation;
    bool isMoving = false;
    float angle;
    public float fireSpeed;
    bool hasDeactivated;

    public AudioClip gunshot;
    public AudioSource SFX;

    // KK
    public enum ProjectileState { Launchable, NotLaunchable };
    private ProjectileState state;

    [Header("Status only")][Tooltip("Changing this will have no effect")]
    public ProjectileState _state;
    //public int temp;
    // KK


	// Use this for initialization
	void Start () {
        hasDeactivated = false;
        angle = 0;
        //origin = new Vector2(originPoint.transform.position.x, originPoint.transform.position.y);
        //upperDirection = new Vector2(originPoint.transform.position.x - upperPoint.transform.position.x, originPoint.transform.position.y - upperPoint.transform.position.y);
        //Physics2D.Raycast(origin, upperDirection, 10.0f);
        Debug.DrawLine(originPoint.transform.position, upperPoint.transform.position,Color.red,Mathf.Infinity);
        Debug.DrawLine(originPoint.transform.position,botPoint.transform.position, Color.green, Mathf.Infinity);
        state = ProjectileState.NotLaunchable;
        flowchart.SetBooleanVariable("isShooting", isMoving);

        if (!SFX)
            SFX = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        angle = progressSlider.value * turnRatio;
        //Debug.Log(progressSlider.value);
        if (angle >= 120 && angle <= 210 && state == ProjectileState.NotLaunchable)
        {
          

            flowchart.SendFungusMessage("readyToLaunch");
            state = ProjectileState.Launchable;
        }
        else if (angle < 120 || angle > 210 && state == ProjectileState.Launchable)
        {
           
            flowchart.SendFungusMessage("stop");
            state = ProjectileState.NotLaunchable;
        }
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
        //Debug.Log(rotation.eulerAngles);
        //Fungus.iTween.RotateTo(gameObject, rotation.eulerAngles, 0.1f);


        if (isMoving&&!hasDeactivated)
        {
            //flowchart.SendFungusMessage("disable slider");
            hasDeactivated = true;
        }
        _state = state;
        /*if (solvingTime == true)
            rotateHandler();*/
       


	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "extinguisher")
            flowchart.SendFungusMessage("extinguisher");
        else if (col.name == "photo")
            flowchart.SendFungusMessage("photo");
        else if (col.name == "cops")
            flowchart.SendFungusMessage("cops");
        else if (col.name == "lamp")
            flowchart.SendFungusMessage("lamp");
    }

    void OnMouseDown()
    {
        Debug.Log("mouseDown");
       
        if(state==ProjectileState.Launchable && isMoving == false)
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right * fireSpeed);
            isMoving = true;
            flowchart.SetBooleanVariable("isShooting", isMoving);
            flowchart.SendFungusMessage("disable slider");
            SFX.PlayOneShot(gunshot);
        }
            
        
    }



}
