using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FrameSeqence_handler : MonoBehaviour
{
    // Music and SFX
    public SnapshotSwitcher switchback;
    public AudioClip puzzleSolved;
    public AudioSource puzzleSolved_SFX;

    public SpriteRenderer[] frames;
    public SpriteRenderer[] captions;
    private int startPos;
    public static int currentPos;    
       
    public static bool leftButtonClicked, rightButtonClicked,solved=false;
    
    public Ch3P7_Manager pageManager;

    [Range(1, 5)]
    public float lerpTime = 1f;
    float currentLerpTime;

    public SpriteRenderer child_refreshButton;

    // Use this for initialization
    void Start ()
    {
        startPos = 0;
        currentPos = startPos;
        leftButtonClicked = false;
        rightButtonClicked = false;       
        StartCoroutine(Fader());
    }

    void Update()
    {
        if (FrameSeqence_handler.currentPos==10 && !solved)
        {
            solved = true;
            print("Puzzle solved");
            pageManager.SetSolved();

            //SFX
            puzzleSolved_SFX.PlayOneShot(puzzleSolved);

            // Music
            if (switchback)
                switchback.Switch();
                      
        }
    }
	
    public IEnumerator Fader()
    {
            currentLerpTime = 0;
            Debug.Log("Fader Coroutine started");        
            float inverseLerpTime = 1 / lerpTime;

            while (frames[currentPos].color.a < 1)
            {
                //--------------------------------------------------responsible for FADING IN frames------------------------------------------
                //increment timer once per frame
                currentLerpTime += Time.deltaTime;
                float perc = currentLerpTime * inverseLerpTime;
                if (currentLerpTime > lerpTime)
                {
                    currentLerpTime = lerpTime;
                }

                //------------------------------------------------fades IN the current frame into the scene--------------------------------------
                frames[currentPos].color = Color.Lerp(frames[currentPos].color, new Color(255, 255, 255, 1), perc);

                //------------------------------------------------fades OUT the refresh button---------------------------------------------------
                child_refreshButton.color = new Color(255, 255, 255, 0.75f);
                

                //------------------------------------------------fades IN speech bubbles
                captions[currentPos].color = Color.Lerp(captions[currentPos].color, new Color(255, 255, 255, 1), perc);
              

                //-----------------------------------------------fades OUT the next/previous frames and speech bubbles
                if (leftButtonClicked)
                    {
                        frames[currentPos + 1].color = Color.Lerp(frames[currentPos + 1].color, new Color(255, 255, 255, 0), perc-0.05f);
                        captions[currentPos + 1].color = Color.Lerp(captions[currentPos + 1].color, new Color(255, 255, 255, 0), perc - 0.05f);                        
                    }

                if (rightButtonClicked)
                    {
                        print("LeftButtonClicked");
                        frames[currentPos - 1].color = Color.Lerp(frames[currentPos - 1].color, new Color(255, 255, 255, 0), perc - 0.05f);
                        captions[currentPos - 1].color = Color.Lerp(captions[currentPos - 1].color, new Color(255, 255, 255, 0), perc - 0.05f);
                    }               
                               
            yield return null;
            }


            Debug.Log("Fade complete for: " + frames[currentPos].name + " alpha:" + frames[currentPos].color.a);
            print(currentPos);

            //-----------------------------------reset the buttons
            rightButtonClicked = false;
            leftButtonClicked = false;
       } 



}
