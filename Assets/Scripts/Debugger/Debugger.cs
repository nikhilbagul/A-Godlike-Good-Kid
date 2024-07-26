using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Debugger : MonoBehaviour
{
    public static Utils.VoidBool ToggleDebugSwitch;
    public delegate void Logger(string message, Component caller = null);
    public static Logger LogMessage;
    [Range(1.0f, 10.0f)]
    public float buttonPressTimeout = 4f;
    [Range(2.0f, 5.0f)]
    public float loggerDuration = 3f;
    public string debugCameraName = "Debugger Camera", controlsName = "Controls";

    private string[] logBuffer = new string[2];
    private bool loggerActive = false, cameraSizeNotSet = true;
    private float elapsed = 0;
    private Camera debuggerCamera;

    [SerializeField]
    private Text GUID;
    [SerializeField]
    private Toggle sendTelemetry;
    [SerializeField]
    private Dropdown selector;

    void Start ()
    {
        transform.Find(controlsName).GetComponentInChildren<Slider>().value = GameManager.GetTimeScale();

        debuggerCamera = transform.GetComponentInChildren<Camera>();

        
        Invoke("DelayedStart", 1f);
        
    }

    void DelayedStart()
    {
        //transform.GetComponentInChildren<Toggle>().isOn = GameManager.Instance.sendTelemetry;
        sendTelemetry.isOn = GameManager.Instance.sendTelemetry;
        GUID.text = Persistor.Load().GetGUID;
    }

    void OnEnable ()
    {
        ToggleDebugSwitch += ToggleDebugCamera;
        LogMessage += _LogMessage;
    }

    void OnDisable()
    {
        ToggleDebugSwitch -= ToggleDebugCamera;
        LogMessage -= _LogMessage;
    }

    void ToggleDebugCamera(bool toggle)
    {
        transform.Find(debugCameraName).gameObject.SetActive(toggle);
        transform.Find(controlsName).gameObject.SetActive(toggle);
    }

    void _LogMessage(string message, Component caller = null)
    {
        if (caller)
            message = caller.name + ": " + message;

        Debug.Log(message);
        loggerActive = true;

        // Buffer
        if (logBuffer[0] == null)
            logBuffer[0] = message;
        else 
        {
            if (logBuffer[1] != null)
                logBuffer[0] = logBuffer[1];
            logBuffer[1] = message;
            message = logBuffer[0] + "\n" + message;
        }
        // Buffer

        transform.Find("Logger").GetComponentInChildren<Text>().text = message;
        elapsed = 0;
    }

    void EraseLog()
    {
        string message = "";
        if ((elapsed += Time.unscaledDeltaTime) <= loggerDuration)
            return;

        // Buffer
        if (logBuffer[1] != null)
        {
            logBuffer[0] = logBuffer[1];
            logBuffer[1] = null;
            elapsed = 0;
        }
        else
        {
            logBuffer[0] = null;
            loggerActive = false;
        }
        message = logBuffer[0];
        // Buffer

        transform.Find("Logger").GetComponentInChildren<Text>().text = message;
    }

    void Update ()
    {
        if (loggerActive)
            EraseLog();

        if (cameraSizeNotSet)
        {
            debuggerCamera = transform.GetComponentInChildren<Camera>();
            if (debuggerCamera)
                if (debuggerCamera.gameObject.activeSelf)
                {
                    cameraSizeNotSet = false;
                    debuggerCamera.orthographicSize = Camera.main.orthographicSize;
                }
        }
    }

    public void SetTimeScale()
    {
        float value = transform.Find("Controls").GetComponentInChildren<Slider>().value;
        LogMessage("Time scale set to " + value.ToString("F2"));
        GameManager.SetTimeScale(value);
    }

    public void ResetTimeScale ()
    {
        transform.Find("Controls").GetComponentInChildren<Slider>().value = 1;
        GameManager.SetTimeScale(1);
    }
    
    public void Skip ()
    {
        LogMessage("Skip requested");
        GameManager.Instance.Skip();
    }

    public void Rewind()
    {
        GameManager.Instance.Rewind();
    }

    public void DeleteSaveFile()
    {
        Persistor.Delete();
    }

    public void ToggleTelemetry(bool setTrue)
    {
        GameManager.Instance.sendTelemetry = sendTelemetry.isOn;
    }

    public void LoadScene()
    {
        if (selector.value > 0)
        {
            string[] scenes = { "Loading Screen", "Ch1_Title", "Ch2_Title", "Ch3_Title" };
            SceneManager.LoadScene(scenes[selector.value - 1]);
        }
    }
}
