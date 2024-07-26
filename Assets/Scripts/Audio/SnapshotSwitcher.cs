using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class SnapshotSwitcher : MonoBehaviour
{
    public AudioMixerSnapshot snapshot;
    [Range(0,10)]
    public float crossfade;

    public void Switch()
    {
        if (snapshot)
            snapshot.TransitionTo(crossfade);
    }
}
