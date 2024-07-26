using UnityEngine;
using System.Collections;
using Fungus;

public class Ch2_P5 : PageManager {

    public Flowchart flowchart;
    bool correctDraw = false;
    bool wrongDraw = false;
    public GameObject FayeAndGoatman;
   
    // Use this for initialization
    protected override void Start () {
        base.Start();

	}


    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        correctDraw = flowchart.GetBooleanVariable("correct");
        if (correctDraw == true)
        {
            flowchart.SendFungusMessage("shouldMoveOn");
            //GetComponent<Draggable2D>().dragEnabled = true;
            flowchart.SetBooleanVariable("correct", false);
        }
        wrongDraw = flowchart.GetBooleanVariable("wrong");
        if (wrongDraw == true)
        {
            flowchart.SendFungusMessage("failState");
            flowchart.SetBooleanVariable("wrong", false);
        }

    }

    public void enableDrag()
    {
        FayeAndGoatman.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void disableDrag()
    {
        FayeAndGoatman.GetComponent<BoxCollider2D>().enabled = false;
    }
}
