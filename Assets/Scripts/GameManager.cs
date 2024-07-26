using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static readonly string _GameVersion = "v00.02.04";

    [Header("Prefabs")]
    public GameObject _debugger;
    public GameObject _UICanvas;

    [Header("Resources")]
    public AudioClip[] pageFlip;

    [Header("Variables")]
    public bool debug = true;
    [SerializeField]
    public bool sendTelemetry = true;

    [SerializeField]
    private bool aspectRatioAware = true;
    public bool AspectRatioAware { get { return aspectRatioAware; } }

    [HideInInspector]
    public GameObject debuggerParent, UICanvas;

    public static event Utils.Void TurnPage;
    public static PageManager currentPageManager;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private static bool nextEnabled = false;
    public static bool NextEnabled { get { return nextEnabled; } }

    private bool _debug, dogearEnabled = false, cameraSizeSet = false;
    public void UnsetCamera() { cameraSizeSet = false; }
    private AudioSource audioSource;

    private float totalTime = 0;
    public float TotalTime { get { return totalTime; } }
    public void ResetTotalTime() { totalTime = 0; }

    void Awake ()
    {
        // Singleton
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        // Singleton
    }

    void Start ()
    {
        //InstantiatePrefabs();
        _debug = !debug;
        audioSource = GetComponent<AudioSource>();

        if (!currentPageManager)
            InitScene();
    }

    // Begin new scene
    public void InitScene()
    {
        if (aspectRatioAware && !cameraSizeSet)
        {
            float ratio = (float)Screen.width / (float)Screen.height;
            float newSize = Camera.main.orthographicSize;
            
            Debug.Log(newSize);


            /* 
             * Assuming linear transposition
             *  y = mx + c
             *  where y and x are new and old values of orthographic camera size, respectively.
             *  By trial and error, 5 (Loading Page) transposes to ~7, 10.24 (most pages) transposes to ~14.
             *  Therefore, y = 1.32x + 0.3
             */
            // 9/16: 
            if (ratio <= 0.6f) newSize = (Camera.main.orthographicSize * 1.32f) + 0.3f;

            /* More steps in aspect ratios will require more prefabs. 
             * Sticking to only two for now: 3/4 (.75) and 9/16 (.56). 
             */
            //// 10/16
            //else if (ratio <= 0.7f) coeff = 1.4f;

            Camera.main.orthographicSize = newSize;

            cameraSizeSet = true;
        }

        InstantiatePrefabs();
        ToggleNext(false);
        FindObjectOfType<Fader>().FadePageIn();
        if (Debugger.ToggleDebugSwitch != null)
            Debugger.ToggleDebugSwitch(debug);

        Debugger.LogMessage("New scene triggered");
    }

    void InstantiatePrefabs()
    {
        Debugger _debuggerParent = FindObjectOfType<Debugger>();
        if (_debuggerParent)
            Destroy(_debuggerParent.gameObject);
        debuggerParent = Instantiate(_debugger);
        debuggerParent.name = "Debugger Tools";

        UICanvas = GameObject.Find("UI Canvas");
        if (UICanvas)
            Destroy(UICanvas);
        UICanvas = Instantiate(_UICanvas);
        UICanvas.name = "UI Canvas";
        INextPageHandler nph = UICanvas.GetComponentInChildren(typeof(INextPageHandler)) as INextPageHandler;
        nph.ToggleDogear(false);
    }

    void Update ()
    {
        // Enable/disable debugger. Comment to disable completely.
        DebuggerTriggerListener();

        //elapse time 
        totalTime += Time.deltaTime;
    }

    // Is called on every frame. Sets debug = _debug at the end. If they are different at the start, fires corresponding switch.
    void DebuggerTriggerListener()
    {
        if (debug != _debug)
        {
            Debug.Log("Debugging turned " + (debug ? "on" : "off") + ".");
            if (Debugger.ToggleDebugSwitch != null)
                Debugger.ToggleDebugSwitch(debug);
        }
        _debug = debug;
    }

    public void ToggleNext(bool setEnabled)
    {
        nextEnabled = setEnabled;
        NPHManual nph = null;
        if (UICanvas)
            nph = UICanvas.GetComponentInChildren<NPHManual>();
        if (nph != null)
            nph.ToggleDogear(setEnabled);

        if (setEnabled)
            if (!currentPageManager.IsSolved)
                nph.SetFailed();
    }

    public void Rewind()
    {
        SceneManager.LoadScene(Mathf.Max(SceneManager.GetActiveScene().buildIndex - 1, 0));
    }

    // Debugger Next
    public void Skip()
    {
        if (currentPageManager)
        {
            cameraSizeSet = false;
            currentPageManager.LoadNextPage(true);
        }
    }    

    // Asks Page Manager to load next page
    public void AttemptNext (bool advance=true) { if(nextEnabled) StartCoroutine(_Next(advance)); }        // Helper
    IEnumerator _Next(bool advance)
    {
        ToggleNext(false);
        cameraSizeSet = false;

        // Start page fade out
        Fader fader = FindObjectOfType<Fader>();
        fader.FadePageOut();
        if (TurnPage != null)
            TurnPage();

        if (pageFlip.Length > 0)
            audioSource.PlayOneShot(pageFlip[Random.Range(0, pageFlip.Length)]);

        // Wait for fade out to complete
        while (!fader.IsFadeComplete())
            yield return null;

        // Attempt load
        try
        {
            currentPageManager.LoadNextPage((!currentPageManager.IsSolved ? false : advance));
            Debugger.LogMessage("Page swiped.");
        }
        catch (System.Exception e)
        {
            Debugger.LogMessage(e.Message, this);
        }
    }



    // Debug Tools
    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public static float GetTimeScale()
    {
        return Time.timeScale;
    }

    public void SendTotalTime()
    {
        //DataPoster.Instance.SendUserData(game_time_url+GUID+totalTime.ToString()+System.DateTime.Now.ToString());
#if UNITY_EDITOR
        URLSetter.GameTime(totalTime, true);
#else
        URLSetter.GameTime(totalTime);
#endif
        ResetTotalTime();
    }

    // On reaching Credits
    public void ObliterateCheckpoint()
    {
        Persistor.ObliterateCheckpoint();
    }
   
}
