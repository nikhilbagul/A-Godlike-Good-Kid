using UnityEngine.SceneManagement;

public class Test2PageManager : PageManager
{
    void Awake()
    {
        Invoke("EnableNext", 4);
    }

    void EnableNext()
    {
        GameManager.Instance.ToggleNext(true);
    }

    void OnEnable ()
    {
        CutscenePlayer.Callback += IncrementSceneCounter;
    }

    void OnDisable()
    {
        CutscenePlayer.Callback -= IncrementSceneCounter;
    }

    void IncrementSceneCounter ()
    {
        Fungus.Flowchart flowchart = GetComponent<Fungus.Flowchart>();
        flowchart.SetIntegerVariable("Scene", flowchart.GetIntegerVariable("Scene") + 1);
        flowchart.SendFungusMessage("Next");
    }

    public void NextPage()
    {
        SceneManager.LoadScene(0);
    }
}
