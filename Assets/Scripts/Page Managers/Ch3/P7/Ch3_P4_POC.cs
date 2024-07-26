using UnityEngine;
using System.Collections;

public class Ch3_P4_POC : PageManager
{
    // This is the Start() method
    protected override void Start ()
    {
        /* DON'T TOUCH */
        base.Start();
        /* DON'T TOUCH */

        // Enter anything you would in Start() here
        panelPositions = new Vector3[currentSequence.Length];
        swapper = new GameObject[2];
        int i = 0;
        foreach (GameObject obj in currentSequence)
            panelPositions[i++] = obj.transform.position;

        syncCounter = counter = 0;

        audioSource = GetComponent<AudioSource>();

        GameObject f = GameObject.Find("Carryover SFX");
        if (!f)
        {
            f = Instantiate(fireLoop);
            f.name = "Carryover SFX";
            AudioSource a = f.GetComponent<AudioSource>();
            a.volume = 1;
            a.Play();
        }


        fire.SetActive(false);
        fullPuzzle.SetActive(false);

        //GameManager.TurnPage += FindObjectOfType<CarryoverSFX>().FadeOutAndDestroy;

        StartCoroutine(PlayIntro());
    }

    // Call this when next page is to be enabled
    void EnableNext()
    {
        SetSolved();
    }

    public GameObject[] currentSequence;
    public GameObject[] solutionSequence;
    public GameObject[] startCutscene;
    public GameObject startCutsceneFader, fullPuzzle;
    public Sprite[] endCutscene;
    public GameObject fire;
    public GameObject fireLoop;
    public float switchPanelDelay = 2;
    public AudioClip solvedSound, bigFire;
    public bool useIntro = false;

    private bool panelsSolved = false, sketchbookSolved = false, stopCounting = false, stopSwitching = false;
    
    // counter = current unpanelsSolved panel, syncCounter follows counter within quantized ticks (0.5 s)
    private int counter, syncCounter;
    private float elapsed = 0;
    private static Vector3[] panelPositions;
    private static GameObject[] swapper;
    private AudioSource audioSource;

    IEnumerator PlayIntro()
    {
        if (useIntro)
        {
            Ch3P4_Cutscene cutscene = startCutsceneFader.GetComponent<Ch3P4_Cutscene>();

            startCutsceneFader.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            // Set all intro panels invisible
            Color invisible = new Color(1, 1, 1, 0);
            for (int i = 0; i < startCutscene.Length; ++i)
                startCutscene[i].GetComponent<SpriteRenderer>().color = invisible;

            // Fade panels in
            for (int i = 0; i < startCutscene.Length; ++i)
            {
                StartCoroutine(cutscene.FadeIn(startCutscene[i].GetComponent<SpriteRenderer>()));
                //yield return new WaitForSeconds(switchPanelDelay);
                while (cutscene.fading)
                    yield return null;
            }
            yield return new WaitForSeconds(switchPanelDelay);

            // Disable panels and fadeout to puzzle
            for (int i = 0; i < startCutscene.Length; ++i)
                startCutscene[i].SetActive(false);
            StartCoroutine(cutscene.FadeOut(startCutsceneFader.GetComponent<SpriteRenderer>()));
        }
        fullPuzzle.SetActive(true);
    }

    void OnEnable()
    {
        MovingPanel.onClickDelegate += Swap;
        GameManager.TurnPage += DestroyFire;
    }

    void OnDisable()
    {
        MovingPanel.onClickDelegate -= Swap;
        GameManager.TurnPage -= DestroyFire;
    }

    void DestroyFire()
    {
        FindObjectOfType<CarryoverSFX>().FadeOutAndDestroy();
    }

    public override void LoadNextPage(bool advance = true)
    {
        Destroy(GameObject.Find("Carryover SFX"));
        base.LoadNextPage(advance);
    }

    public void SetSketcbookSolved()
    {
        ++syncCounter;
        sketchbookSolved = true;
        stopSwitching = false;
        GameObject.Find("Sketchbook").gameObject.SetActive(false);
    }

    void SynchronousCounter()
    {
        // Cooldown
        elapsed += Time.deltaTime;
        if (elapsed < switchPanelDelay)
            return;

        elapsed = 0;

        // Catch up to instantaneous counter
        if (counter > syncCounter)
        {
            if (!stopSwitching)
                currentSequence[syncCounter].GetComponentInChildren<MovingPanel>().DelayedSwitchSprite();

            if (syncCounter > 0)
            {
                if (!stopSwitching)
                    currentSequence[syncCounter - 1].GetComponentInChildren<MovingPanel>().DelayedSwitchSprite();

                // Fire
                if (syncCounter == 3)
                {
                    fire.SetActive(true);
                    if (bigFire)
                    {
                        GameObject fireLoop = GameObject.Find("Carryover SFX"); // transform.Find("Fire Loop").gameObject;
                        AudioSource audio = fireLoop.GetComponent<AudioSource>();
                        audio.clip = bigFire;
                        audio.Play();
                    }
                }

                if (!sketchbookSolved)
                {
                    GameObject sketchbook;
                    try
                    {
                        string sketchbookPath = currentSequence[syncCounter].transform.GetChild(0).name + "/Sketchbook";
                        sketchbook = currentSequence[syncCounter].transform.Find(sketchbookPath).gameObject;
                        sketchbook.GetComponent<Collider2D>().enabled = true;
                        --syncCounter;
                        stopSwitching = true;
                    }
                    catch (System.Exception)
                    {
                        //Debug.Log("Error finding sketchbook or its collider.");
                    }
                }

            }
            ++syncCounter;
        }

        // Set complete after 1.5 s
        if (syncCounter == currentSequence.Length)
        {
            stopCounting = true;
            StartCoroutine(PlayEndCutscene());
            //SetComplete();
        }
    }

    IEnumerator PlayEndCutscene()
    {
        SpriteRenderer lastPanel = solutionSequence[solutionSequence.Length - 1].GetComponentInChildren<SpriteRenderer>();
        AudioSource fireLoop = GameObject.Find("Carryover SFX").GetComponent<AudioSource>();
        for (int i = 0; i < endCutscene.Length; ++i)
        {
            //fireLoop.volume -= (float)1 / endCutscene.Length;
            yield return new WaitForSeconds(switchPanelDelay - 0.5f);
            lastPanel.sprite = endCutscene[i];
        }
        //Destroy(fireLoop.gameObject);
        SetSolved();
    }

    void Update()
    {
        if (!stopCounting)
            SynchronousCounter();

        if (counter < solutionSequence.Length)
        {
            // Check for correct panel and increment counter till over
            if (currentSequence[counter] == solutionSequence[counter])
            {
                currentSequence[counter].GetComponentInChildren<MovingPanel>().stateMachine.triggers.setFixed = true;
                ++counter;
            }
        }
        else
        {
            if (!panelsSolved)
            {
                foreach (GameObject panel in currentSequence)
                {
                    panel.GetComponentInChildren<MovingPanel>().stateMachine.triggers.setSolved = true;
                }
                audioSource.PlayOneShot(solvedSound);
                panelsSolved = true;
            }
        }
    }

    public void Swap(GameObject panel)
    {
        if (swapper[0] == null)
        {
            Debug.Log(panel.name + " called Swap");
            swapper[0] = panel;
        }
        else
        {
            // Check for click on same panel
            if (swapper[0] == panel)
            {
                swapper[0] = null;
                Debug.Log("Same panel clicked.");
                return;
            }

            // Swap
            swapper[1] = panel;
            Vector3 pos1 = swapper[0].transform.position;
            Vector3 pos2 = swapper[1].transform.position;

            for (int i = 0; i < currentSequence.Length; ++i)
            {
                GameObject temp;
                if (currentSequence[i] == swapper[0])
                {
                    temp = currentSequence[i];
                    for (int j = 0; j < currentSequence.Length; ++j)
                    {
                        if (currentSequence[j] == swapper[1])
                        {
                            currentSequence[i] = currentSequence[j];
                            currentSequence[j] = temp;
                            break;
                        }
                    }
                    break;
                }
            }

            swapper[0].transform.position = pos2;
            swapper[1].transform.position = pos1;

            swapper[0].GetComponentInChildren<MovingPanel>().stateMachine.triggers.setMoveable = true;
            swapper[1].GetComponentInChildren<MovingPanel>().stateMachine.triggers.setMoveable = true;

            swapper[0] = swapper[1] = null;
        }
    }
}
