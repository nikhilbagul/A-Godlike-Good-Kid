using UnityEngine;
using System.Collections;

public class Ch1_P2 : PageManager
{
    public float cutsceneFF = 3;
    public AudioClip solved;

    public enum State { Intro, Puzzle, Outro, Completed };

    [Header("Status")]
    public State state;

    private float origTimeout;
    private Fungus.Flowchart cutscenePlayer;
    private AudioSource self;

    void OnEnable()
    {
        GameManager.TurnPage += DestroyAlarm;
    }

    void OnDisable()
    {
        GameManager.TurnPage -= DestroyAlarm;
    }

    void DestroyAlarm()
    {
        FindObjectOfType<CarryoverSFX>().FadeOutAndDestroy();
    }

    protected override void Start()
    {
        base.Start();

        if (!cutscenePlayer)
            cutscenePlayer = GameObject.Find("Cutscene Flowchart").GetComponent<Fungus.Flowchart>();
        if (!cutscenePlayer)
            Debug.Assert(false, "Cutscene Flowchart not found");

        self = GetComponent<AudioSource>();

        origTimeout = cutscenePlayer.GetComponentInParent<CutscenePlayer>().sceneTimeout;

        SnapshotSwitcher s = FindObjectOfType<SnapshotSwitcher>();
        if (s)
            s.Switch();
    }

    void Update()
    {
        state = (State)cutscenePlayer.GetIntegerVariable("State");
    }

    public void PuzzleSolved()
    {
        state = State.Outro;
        if (self)
            self.PlayOneShot(solved);
        cutscenePlayer.SendFungusMessage("ShowMap");
    }

    public void ModifyCutsceneSpeed(bool speedUp)
    {
        CutscenePlayer cp = FindObjectOfType<CutscenePlayer>();
        if (speedUp)
            cp.SpeedFast();
        else
            cp.SpeedNormal();
    }
}
