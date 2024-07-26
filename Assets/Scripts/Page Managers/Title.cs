using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Title : PageManager
{
    [SerializeField]
    private float delay;
    public CanvasGroup swipeToStart;
    public AudioMixerSnapshot start;

	protected override void Start ()
    {
        Invoke("DelayedNext", delay);
        base.Start();
        if (start)
            start.TransitionTo(2);
        
	}

   

    void DelayedNext()
    {
        if (swipeToStart)
            swipeToStart.alpha = 1;
        SetSolved();

        GameManager.Instance.ResetTotalTime();
    }
	
}
