public class TestPageManager : PageManager
{
    protected override void Start ()
    {
        base.Start();
        Invoke("EnableNext", 4);
    }

    void EnableNext()
    {
        SetSolved();
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
}
