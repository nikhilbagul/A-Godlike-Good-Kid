using UnityEngine;
using System.Collections;

public class DataPoster : MonoBehaviour {

    private static DataPoster instance;
    public static DataPoster Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

	public void SendUserData(string url)
    {
        if (GameManager.Instance.sendTelemetry)
        {
            WWW www = new WWW(url);
            StartCoroutine(waitForRequest(www));
        }
    }

    IEnumerator waitForRequest(WWW www)
    {
        yield return www;

        if(www.error == null)
        {
            Debugger.LogMessage("<color='green'>Telemetry data sent " + www.text + "</color>");
        }
        else
        {
            Debugger.LogMessage("<color='red'>Failed to send telemetry data: " + www.error + "</color>");
        }
    }

}
