using UnityEngine;
using System.Collections;
using DG.Tweening;

public class leftButton_handler : MonoBehaviour
{
    //SFX
    public AudioClip clocktick;
    public AudioSource clocktick_SFX;

    public GameObject targetObject;
    public SpriteRenderer child_yellowglow, child_refreshButton;
    Sequence antiClockwise;
    private Quaternion currentRotation;
    private Color glow_color;
    private bool isSequenceRunning = false;

    public SpriteRenderer leftButtonHighlighter, leftButton;

    // Use this for initialization
    void Start()
    {
        leftButton = transform.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (FrameSeqence_handler.currentPos == 0)
        {
            leftButton.color = new Color(255, 255, 255, 0.66f);
        }

        else
        {
            leftButton.color = new Color(255, 255, 255, 1);
        }
    }

    void OnMouseDown()
    {
        if (!isSequenceRunning && FrameSeqence_handler.currentPos <= 10 && FrameSeqence_handler.currentPos > 0)            
        {
            leftButtonHighlighter.color = new Color(255, 255, 255, 1);          //clockhand highlighter

            currentRotation = targetObject.transform.rotation;
            antiClockwise = DOTween.Sequence();
            antiClockwise.Insert(0, targetObject.transform.DORotate(currentRotation.eulerAngles + new Vector3(0, 0, 36), 0.5f, RotateMode.Fast)).OnComplete(disableGlow);

            FrameSeqence_handler.leftButtonClicked = true;
            FrameSeqence_handler.currentPos = FrameSeqence_handler.currentPos - 1;      //increment the current frame position in the array
            FrameSeqence_handler fsh = FindObjectOfType<FrameSeqence_handler>();
            StartCoroutine(fsh.Fader());

            child_yellowglow.color = new Color(255, 255, 255);
          
            //print("Left pressed");

            isSequenceRunning = true;
        }


        
    }

    void OnMouseUp()
    {
        leftButtonHighlighter.color = new Color(255, 255, 255, 0);       
    }

    void disableGlow()
    {
        child_yellowglow.color = new Color(255, 255, 255, 0);
        child_refreshButton.color = new Color(255, 255, 255, 1);
        isSequenceRunning = false;

        clocktick_SFX.PlayOneShot(clocktick);


    }




}
