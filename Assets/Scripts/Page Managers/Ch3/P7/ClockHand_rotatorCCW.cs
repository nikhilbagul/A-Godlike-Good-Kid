using UnityEngine;
using System.Collections;
using Fungus;
using System.Collections.Generic;
using DG.Tweening;

public class ClockHand_rotatorCCW : MonoBehaviour
{
    private Color glow_color, temp_color;   
    public SpriteRenderer[] frames;
    private int i, current_frame, seq_number;
    private float alpha, angle;
    public bool clockwise, counter_clockwise;
    private Vector3 ref_vector;
    private Vector2 offset;
    public Flowchart flowchart;
    public SpriteRenderer child_redglow, child_yellowglow;
    Sequence antiClockwise;
    


    void Start()
    {
        temp_color = frames[0].color;
        i = 0;
        current_frame = 0;
        glow_color = child_yellowglow.color;
        child_yellowglow.color = glow_color;
        offset = Camera.main.WorldToScreenPoint(transform.GetComponent<BoxCollider2D>().offset);
        ref_vector = new Vector3(offset.x, offset.y, 0);
        //Vector3 size= Camera.main.WorldToScreenPoint(transform.GetComponent<BoxCollider2D>().size);
        //ref_vector.x = ref_vector.x + (size.x/2);
    }

    void Update()
    {
       current_frame = (int) transform.rotation.eulerAngles.z / 90;      // checks in which frame section the pointer currently is
                                                                         // we have four frames in anti clockwise puzzle, hence we divide by 360/4 = 90
        if (current_frame >= i)
        {
            i = i + 1;
            seq_number = i;
            //Debug.Log(current_frame);
        }
       if (current_frame < i)
          {
            i = i - 1;
            seq_number = i;
            //Debug.Log(current_frame);
        }

        if (transform.rotation.eulerAngles.z > 315 && transform.rotation.eulerAngles.z < 360)       // if the puzzle is about to be solved, snap to 12 o clock
        {
            seq_number = 4;
        }

        //alpha_angle = transform.rotation.eulerAngles.z;
        //alpha = (amplitude / time_period) * (time_period - Mathf.Abs(alpha_angle % (2 * time_period) - time_period));

        //using traingle wave to change alpha of frames according to pointer's rotation
        alpha = (1f / 45f) * (45f - Mathf.Abs(transform.rotation.eulerAngles.z % (2 * 45f) - 45f));     
        
        temp_color.a = alpha;
        frames[i].color = temp_color;       

    }

    void OnMouseDown()
    {
        Vector3 mouseDragStartPos = Input.mousePosition;
        
        if (mouseDragStartPos.x < ref_vector.x)             // check if the player has started rotating in clockwise or antiCW direction
        {                                                   // NOTE : this only checks for direction at START of a drag/rotation, NOT during drag/rotation
            clockwise = false;
            counter_clockwise = true;
            //print("Left !!!");
        }

        if (mouseDragStartPos.x >= ref_vector.x)
        {
            clockwise = true;
            counter_clockwise = false;
            //print("Right !!!");
        }
    }

    void OnMouseDrag()
    {
        
        //transform.Rotate(new Vector3(0,0, Mathf.Sqrt(Input.GetAxis("Mouse X") * Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y") * Input.GetAxis("Mouse Y"))));  
        
             
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;                                      

       if (counter_clockwise)
        {
            angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;           // if CCW direction of rotation, calculate angle of rotation with backward rotation enabled
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            /*
            if (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f > 0)
            {
                angle = Mathf.Max(angle, Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }


            if (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f < 0)
            {
                angle = Mathf.Max(angle, 360 + (Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90));
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            */
        }            
       

        glow_color.a = 1f;
        child_yellowglow.color = glow_color;                           
    }

    void OnMouseUp()
    {
        glow_color.a = 0f;
        child_yellowglow.color = glow_color;
        glow_color.a = 1f;
        child_redglow.color = glow_color;
        angle = 0;                                 //enable this if rotation is Controlled by Angles
        antiClockwise = DOTween.Sequence();
        switch (seq_number)
        {
            case 0:
                antiClockwise.Insert(0, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(antiClockWiseOnComplete);
                Debug.Log("case 0");
                break;
            case 1:
                Debug.Log("case 1");
                antiClockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.Fast));
                antiClockwise.Insert(0.5f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(antiClockWiseOnComplete);
                break;
            case 2:
                Debug.Log("case 2");
                antiClockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.Fast));
                antiClockwise.Insert(0.5f, transform.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.Fast));
                antiClockwise.Insert(1.0f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(antiClockWiseOnComplete);
                break;
            case 3:
                Debug.Log("case 3");
                antiClockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 270), 0.5f, RotateMode.Fast));
                antiClockwise.Insert(0.5f, transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.Fast));
                antiClockwise.Insert(1.0f, transform.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.Fast));
                antiClockwise.Insert(1.5f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(antiClockWiseOnComplete);
                break;
            case 4:
                Debug.Log("case 4");
                antiClockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 0), 0.30f, RotateMode.Fast));
                antiClockwise.InsertCallback(0.0f, antiClockwise_snap);
                break;
        }
       
    }

    public void antiClockwise_snap()
    {
        glow_color.a = 0f;
        child_redglow.color = glow_color;
        flowchart.ExecuteBlock("enableCW");
    }

    public void antiClockWiseOnComplete()
    {
        glow_color.a = 0f;
        child_redglow.color = glow_color;        
        antiClockwise.Kill();        
    }
}





/*
        cross_product = Vector3.Cross(ref_vector, transform.up);
        dot_product = Vector3.Dot(cross_product, transform.forward * -1);
        Debug.Log("Dot Product: " + Vector3.Cross(transform.up, ref_vector));

        if (dot_product >= prev_rotation)
        {
            counter_clockwise = true;
        }

        else
        {
            counter_clockwise = false;
        }
        
        //Debug.Log("Hand Vector: " + hand_vector);
        //Debug.Log("Ref Vector: " + ref_vector);
        */


/*

    float mouseDistance = 0;
    private Vector3 lastPosition;
    private bool trackMouse = false;


        if (Input.GetMouseButtonDown(0))
        {
            trackMouse = true;
            lastPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            Debug.Log("Mouse moved " + mouseDistance + " while button was down.");
            mouseDistance = 0;
        }

        if (trackMouse)
        {
            var newPosition = Input.mousePosition;
            // If you just want the x-axis:
            mouseDistance += (newPosition.x - lastPosition.x);
            // If you just want the y-axis,change newPosition.x to newPosition.y and lastPosition.x to lastPosition.y
            // If you want the entire distance moved (not just the X-axis, use:
            //mouseDistance += (newPosition - lastPosition).magnitude;
            lastPosition = newPosition;
        }
*/


/*
// UNTESTED CODE!!!

void OnMouseDrag ()
{
    FadeFrame(frames[FrameToEdit()]);
}

int FrameToEdit()
{
    float angle = transform.rotation.euler.z;
    return ((int) angle / 90);
}

void FadeFrame(SpriteRenderer spr)
{
    alpha = (amplitude / time_period) * (time_period - Mathf.Abs(angle % (2 * time_period) - time_period));
    temp_color.a = alpha;
    spr.color = temp_color;;
}

// UNTESTED CODE END

*/



/*
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;

    void Start()
    {
        _sensitivity = 0.6f;
        _rotation = Vector3.zero;
    }

    void Update()
    {
        if (_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply rotation
            _rotation.z = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

            // rotate
            transform.Rotate(_rotation);

            // store mouse
            _mouseReference = Input.mousePosition;
        }
    }

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;

        // store mouse
        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
    */

/*
private Vector3 mouse_pos;
private Transform target; //Assign to the object you want to rotate
private Vector3 object_pos;
private float angle;

void Update()
{
}

void OnMouseDrag()
{
    var mouse = Input.mousePosition;
    var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
    var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
    var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle);
}
*/
