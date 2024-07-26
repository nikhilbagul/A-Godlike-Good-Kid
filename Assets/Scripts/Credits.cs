using UnityEngine.SceneManagement;

public class Credits : PageManager
{
    void Awake()
    {
        Invoke("EnableNext", 4);
        isSolved = true;

        Invoke("SendData",1);
    }

    protected override void Start()
    {
        base.Start();
        Debugger.LogMessage(UnityEngine.Application.version);
    }

    void SendData()
    {
        GameManager.Instance.SendTotalTime();
        GameManager.Instance.ObliterateCheckpoint();
    }

    void EnableNext()
    {
        GameManager.Instance.ToggleNext(true);
    }

    public override void LoadNextPage(bool advance = true)
    {
        Debugger.LogMessage("Loading first.");
        GameManager.Instance.UnsetCamera();
        SceneManager.LoadScene(0);
    }
}
