using UnityEngine;
using System.Collections;

public class WallClock_handler : MonoBehaviour
{
    [SerializeField]
    private Fungus.Flowchart flowchart;
    public AudioClip wallClockTap;
    public AudioSource WallClock_SFX;
    void OnMouseDown()
    {
        WallClock_SFX.PlayOneShot(wallClockTap);
        Clock();
    }

    void Update ()
    {
        //if (Input.GetMouseButtonDown(0))
        //    Clock();
    }

    void Clock()
    {
        Debugger.LogMessage("Tap detected.");
        if (flowchart)
            flowchart.ExecuteBlock("Wall_clock");
    }
}
