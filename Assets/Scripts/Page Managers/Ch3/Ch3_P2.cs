using UnityEngine;
using System.Collections;
using Fungus;

public class Ch3_P2 : PageManager
{
    public float minSwipeDistance = 2;
    public Flowchart flowchart;
    public GameObject skull, FayeAndErnest,undergroundSkull, thresholdPosition;
    public PolygonCollider2D poly;
    Animation tombstoneAnimation;
    public bool hasChangedLayer = false;
    Animator animator;
    bool hasClicked = false;

    public AudioClip outdoorAmbience;

    protected override void Start()
    {
        base.Start();
        tombstoneAnimation = GameObject.Find("tombstone 1").GetComponent<Animation>();
        
        skull.GetComponent<Rigidbody2D>().gravityScale = 0;
        if (!FayeAndErnest)
            FayeAndErnest = GameObject.Find("Faye and Ernest");

        NPHManual nph = FindObjectOfType<NPHManual>();
        nph.minDistance = minSwipeDistance;
        MusicPlayer mp = FindObjectOfType<MusicPlayer>();
        if (mp)
            mp.FadeOverlay(1);

        CarryoverSFX c = CarryoverSFX.Instance;
        if (c)
        {
            AudioSource a = c.GetComponent<AudioSource>();
            a.clip = outdoorAmbience;
            a.Play();
        }
    }
    void Update()
    {
        if (undergroundSkull.transform.position.x > thresholdPosition.transform.position.x && !hasChangedLayer)
        {
            hasChangedLayer = true;
            changeLayer();
        }

        if (!hasClicked)
        {
            hasClicked = flowchart.GetBooleanVariable("hasClicked");
            if (!tombstoneAnimation)
                tombstoneAnimation = GameObject.Find("tombstone 1").GetComponent<Animation>();
            tombstoneAnimation.Play();
        }
    }

    public void enableDrag()
    {
        
        FayeAndErnest.GetComponent<Draggable2D>().enabled = true;

       

        Debug.Log(TapToMove.index);
        //if(TapToMove.index >= 1)
        //{
            //flowchart.SendFungusMessage("display hint two");
        //}
        //transform.position = position.transform.position;
    }

    public void enableRigidBodySkull()
    {
        skull.GetComponent<Rigidbody2D>().gravityScale = 1;
        Camera.main.GetComponent<cameraFollow>().interpVelocity = 15;
    }

    public void enableSkullDrag()
    {
        FayeAndErnest.GetComponent<BoxCollider2D>().enabled = false;
        //skull.GetComponent<Draggable2D>().enabled = true;
        skull.GetComponent<Clickable2D>().enabled = true;
        Camera.main.GetComponent<cameraFollow>().target = skull;
        if(poly)
        {
            poly.enabled = true;
        }
    }

    public void disableSkullDrag()
    {
        //skull.GetComponent<Draggable2D>().enabled = false;
        skull.GetComponent<Clickable2D>().enabled = false;
    }

    public void SetSolvedAfterDelay(float delay)
    {
        Invoke("_SetSolved", delay);
    }

    void _SetSolved()
    {
        SetSolved();
    }

    void changeLayer()
    {
        Debug.Log("change the fucking layer");
        undergroundSkull.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    public void PlayLayer2()
    {
        MusicPlayer mp = FindObjectOfType<MusicPlayer>();
        if (mp)
            mp.FadeOverlay(2, 0.8f);
    }
}
