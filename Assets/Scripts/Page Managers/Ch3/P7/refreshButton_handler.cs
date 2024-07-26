using UnityEngine;
using Fungus;
using DG.Tweening;
using System.Collections;

public class refreshButton_handler : MonoBehaviour {


    //SFX
    public AudioClip rewind;
    public AudioSource rewind_SFX;

    Sequence antiClockwise;
    public GameObject targetObject;
    private Quaternion currentRotation;
    public SpriteRenderer child_yellowglow;
    private bool isSequenceRunning = false;
    public SpriteRenderer refreshButtonHighlighter;

    public Flowchart flowchart;

    void OnMouseDown()
    {

        if (!isSequenceRunning && FrameSeqence_handler.currentPos != 0 && FrameSeqence_handler.currentPos != 10)
        {
            if (!rewind_SFX.isPlaying)
            {
                rewind_SFX.PlayOneShot(rewind);
            }

            refreshButtonHighlighter.color = new Color(255, 255, 255, 1);
            currentRotation = targetObject.transform.rotation;
            antiClockwise = DOTween.Sequence();

            if (FrameSeqence_handler.currentPos < 5)
            {
                antiClockwise.Insert(0, targetObject.transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)).OnComplete(disableGlow);
            }
                
            if (FrameSeqence_handler.currentPos >= 5 && FrameSeqence_handler.currentPos<8)
            {
                antiClockwise.Insert(0, targetObject.transform.DORotate(new Vector3(0, 0, 270), 0.25f, RotateMode.Fast));
                antiClockwise.Insert(0.15f,targetObject.transform.DORotate(new Vector3(0, 0, 0), 0.25f, RotateMode.Fast)).OnComplete(disableGlow);
            }

            if (FrameSeqence_handler.currentPos >=8 && FrameSeqence_handler.currentPos < 10)
            {
                antiClockwise.Insert(0, targetObject.transform.DORotate(new Vector3(0, 0, 180), 0.2f, RotateMode.Fast));
                antiClockwise.Insert(0.15f, targetObject.transform.DORotate(new Vector3(0, 0, 270), 0.2f, RotateMode.Fast));
                antiClockwise.Insert(0.20f, targetObject.transform.DORotate(new Vector3(0, 0, 0), 0.2f, RotateMode.Fast)).OnComplete(disableGlow);
            }

            flowchart.ExecuteBlock("Refresher");
            child_yellowglow.color = new Color(255, 255, 255);
            isSequenceRunning = true;
            
        }
        
    }

    void disableGlow()
    {
        child_yellowglow.color = new Color(255, 255, 255, 0);
        refreshButtonHighlighter.color = new Color(255, 255, 255, 0);       
        isSequenceRunning = false;
        FrameSeqence_handler.currentPos = 0;       
    }
}
