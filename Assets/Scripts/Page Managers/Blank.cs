using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Blank : PageManager
{
    [Range(0, 10)]
    public float delay = 1;
    [Header("Optional")]
    [Tooltip("If present, this snapshot will be transitioned to in given delay.")]
    [Range(0, 5)]
    public float transition = 1;
    public AudioMixerSnapshot snapshot;

	protected override void Start ()
    {
        StartCoroutine(DelayedSolve());
        base.Start();
        if (snapshot)
            snapshot.TransitionTo(transition);
	}
	
	IEnumerator DelayedSolve()
    {
        float elapsed = 0;
        while ((elapsed += Time.deltaTime) <= delay)
            yield return null;
        SetSolved();
    }
}
