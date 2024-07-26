using UnityEngine;
using System.Collections;
using Fungus;
using System.Collections.Generic;
using DG.Tweening;

public class ClockHand_rotatorCW : MonoBehaviour
{

    private Color glow_color, temp_color;
    public SpriteRenderer[] frames;
    private int i, current_frame, seq_number;
    private float alpha, angle;
    
    private Vector3 ref_vector;
    private Vector2 offset;
    public Flowchart flowchart;
    public SpriteRenderer child_yellowglow;
    //Sequence Clockwise;

    void Start ()
    {
        temp_color = frames[0].color;
        i = 0;
        current_frame = 0;
        seq_number = 0;
        glow_color = child_yellowglow.color;
        child_yellowglow.color = glow_color;
        offset = Camera.main.WorldToScreenPoint(transform.GetComponent<BoxCollider2D>().offset);
        ref_vector = new Vector3(offset.x, offset.y, 0);
    }
	
	
	void Update ()
    {
        current_frame = (int)transform.rotation.eulerAngles.z / 60;     // checks in which frame section the pointer currently is
                                                                        // 
        if (current_frame >= i)
        {
            i = i + 1;
            seq_number = i;           
        }
        if (current_frame < i)
        {
            i = i - 1;
            seq_number = i;          
        }

        if (transform.rotation.eulerAngles.z > 0 && transform.rotation.eulerAngles.z < 45)
        {
            seq_number = 0;
        }
               
        alpha = (1f / 30f) * (30f - Mathf.Abs(transform.rotation.eulerAngles.z % (2 * 30f) - 30f));
        temp_color.a = alpha;
        frames[i].color = temp_color;
    }

    

    void OnMouseDrag()
    {
        transform.Rotate(new Vector3(0, 0, -Mathf.Sqrt(Input.GetAxis("Mouse X") * Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y") * Input.GetAxis("Mouse Y"))));

        glow_color.a = 1f;
        child_yellowglow.color = glow_color;
    }

    
    void OnMouseUp()
    {
        glow_color.a = 0f;
        child_yellowglow.color = glow_color;
        //glow_color.a = 1f;
        //child_redglow.color = glow_color;
        //angle = 0;                                 //UNCOMMENT this if rotation is Controlled by Angles

        /*
        Clockwise = DOTween.Sequence();
        switch (seq_number)
        {
            case 0:
                Clockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 0), 0.30f, RotateMode.Fast));
                Clockwise.InsertCallback(0.0f, Clockwise_snap);
                Debug.Log("case 0");
                break;
            case 1:
                Debug.Log("case 1");                
                Clockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 120), 0.5f, RotateMode.Fast));
                Clockwise.Insert(0.5f, transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.Fast));
                Clockwise.Insert(1.0f, transform.DORotate(new Vector3(0, 0, 240), 0.5f, RotateMode.Fast));
                Clockwise.Insert(1.5f, transform.DORotate(new Vector3(0, 0, 300), 0.5f, RotateMode.Fast));
                Clockwise.Insert(2.0f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(ClockWiseOnComplete);
                break;
            case 2:
                Debug.Log("case 2");               
                Clockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.Fast));
                Clockwise.Insert(0.5f, transform.DORotate(new Vector3(0, 0, 240), 0.5f, RotateMode.Fast));
                Clockwise.Insert(1.0f, transform.DORotate(new Vector3(0, 0, 300), 0.5f, RotateMode.Fast));
                Clockwise.Insert(1.5f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(ClockWiseOnComplete);
                break;
            case 3:
                Debug.Log("case 3");                
                Clockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 240), 0.5f, RotateMode.Fast));
                Clockwise.Insert(0.5f, transform.DORotate(new Vector3(0, 0, 300), 0.5f, RotateMode.Fast));
                Clockwise.Insert(1.0f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(ClockWiseOnComplete);
                break;
            case 4:
                Debug.Log("case 4");                
                Clockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 300), 0.5f, RotateMode.Fast));
                Clockwise.Insert(0.5f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(ClockWiseOnComplete);
                break;
            case 5:
                Debug.Log("case 5");               
                Clockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(ClockWiseOnComplete);
                break;
            case 6:
                Debug.Log("case 6");
                Clockwise.Insert(0.0f, transform.DORotate(new Vector3(0, 0, 0), 0.30f, RotateMode.Fast)).OnComplete(ClockWiseOnComplete);
                break;
        }
        */

    }
    

    public void Clockwise_snap()
    {
        glow_color.a = 0f;
        //child_redglow.color = glow_color;
        flowchart.ExecuteBlock("Puzzle_Solved");
    }

    public void ClockWiseOnComplete()
    {
        glow_color.a = 0f;
        //child_redglow.color = glow_color;
        //Clockwise.Kill();
    }
}
