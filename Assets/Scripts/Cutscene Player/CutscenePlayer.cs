using UnityEngine;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour
{
    [Header("Player Parameters")]
    public bool respectTimeScale = true;
    public bool controlSaturation = true;
    public bool autoStart = true, verbose = false, isPaused = false;
    [Range(0.1f, 2.0f)]
    public float tapTimeout = 0.8f;
    [Range(1.0f, 6.0f)]
    public float sceneTimeout = 1.8f;

    // Callback will be fired at the start of every new "scene" in the cutscene.
    // If using Fungus Flowchart, check instructions in Cutscene Manager.
    // If using scripts, subscribe a void() to this event that will handle what happens each scene. 
    public static event Utils.Void Callback;

    private int clicks = 0;
    private bool clickable = false, tapCounting = false;
    private float tapElapsed = 0, sceneElapsed = 0, deltaTime, scene = 0, baseSceneTimeout;

    private Slider[] sliders;

    void SetRefs ()
    {
        sliders = transform.Find("Debugger Canvas").GetComponentsInChildren<Slider>();
    }

    void ToggleClickable (bool toggle)
    {
        GetComponent<BoxCollider2D>().enabled = toggle;
    }

	void OnMouseDown ()
    {
        if (verbose)
            Debugger.LogMessage("Click #" + ++clicks, this);
        NextScene();
    }
	
    void Start ()
    {
        if (autoStart)
            NextScene();
        baseSceneTimeout = sceneTimeout;
    }

    public void Play ()
    {
        isPaused = false;
        if (controlSaturation)
        {
            try
            {
                Camera.main.GetComponent<CameraSaturation>().Desaturate();
            }
            catch { }
        }
        NextScene();
    }

    public void Pause (bool setPaused=true)
    {
        if (controlSaturation)
        {
            CameraSaturation cs = Camera.main.GetComponent<CameraSaturation>();
            if (cs)
            {
                if (!setPaused)
                    cs.Desaturate();
                else
                    cs.Saturate();
            }
        }
        isPaused = setPaused;
    }

    public void SpeedFast(float speedFactor = 3)
    {
        sceneTimeout = baseSceneTimeout / 3;
    }

    public void SpeedNormal()
    {
        sceneTimeout = baseSceneTimeout;
    }

    public void SetSolved(bool advance = true)
    {
        GameManager.currentPageManager.SetSolved(advance);
    }

	void Update ()
    {
        deltaTime = respectTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;

        if (!isPaused)
        {
            TapCountdown();
            SceneCountdown();
        }
	}

    // Tap handlers
    void TapCountdown ()
    {
        if (tapCounting)
        {
            tapElapsed += deltaTime;

            if (sliders == null || !sliders[0])
                SetRefs();
            sliders[0].value = tapElapsed / tapTimeout;

            if (tapElapsed >= tapTimeout)
                ResetTapCounting();
        }
    }

    void SetTapCounting ()
    {
        ToggleClickable(false);
        tapCounting = true;
        tapElapsed = 0;
    }

    void ResetTapCounting ()
    {
        ToggleClickable(true);
        if(verbose)
            Debugger.LogMessage("Clicker enabled.");
        tapElapsed = 0;
        tapCounting = false;
    }
    
    // Scene handlers
    void SceneCountdown ()
    {
        sceneElapsed += deltaTime;

        if (sliders == null || !sliders[0])
            SetRefs();
        sliders[1].value = sceneElapsed / sceneTimeout;

        if (sceneElapsed >= sceneTimeout)
            NextScene();
    }

    void NextScene ()
    {
        if(verbose)
            Debugger.LogMessage("Scene #" + ++scene, this);

        if (Callback != null)
            Callback();
        sceneElapsed = 0;
        SetTapCounting();
    }
}
