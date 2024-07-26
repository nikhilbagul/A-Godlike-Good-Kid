using UnityEngine;
using System.Collections;
using GestureRecognizer;
using Fungus;

public class drawing : MonoBehaviour
{

    public Flowchart flowchart;
    public AudioClip sketch;
    
    void OnGestureRecognition(Result r)
    {
        GetComponent<AudioSource>().PlayOneShot(sketch, 0.8f);
        
        if (r.Name == "circle")
        {
            flowchart.SetBooleanVariable("correct", true);
        }

        if (r.Name != "circle")
        {
            flowchart.SetBooleanVariable("wrong", true);
        }
    }

    void OnEnable()
    {
        GestureBehaviour.OnRecognition += OnGestureRecognition;
    }

    void OnDisable()
    {
        GestureBehaviour.OnRecognition -= OnGestureRecognition;
    }

    void OnDestory()
    {
        GestureBehaviour.OnRecognition -= OnGestureRecognition;
    }

    public void EnableDrawing()
    {
        GetComponent<GestureBehaviour>().isEnabled = true;
    }

    public void DisableDrawing()
    {
        GetComponent<GestureBehaviour>().isEnabled = false;
    }
}
