using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance { get { return instance; } }
    public AudioMixerSnapshot[] snapshots;

    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void TransitionTo(AudioMixerSnapshot snapshot, float duration = 1)
    {
        //bool found = false;
        //foreach (AudioMixerSnapshot s in snapshots)
        //    if (s == snapshot)
        //    {
        //        found = true;
        //        break;
        //    }
        //if (found)
            snapshot.TransitionTo(duration);
        //else
        //    Debug.Log("Snapshot parameter not found in array.");
    }

    public void TransitionTo(int index, float duration=1)
    {
        if (index < 0 || index >= snapshots.Length)
        {
            Debug.Log("Index out of range");
            return;
        }

        AudioMixerSnapshot snapshot = snapshots[index];
        TransitionTo(snapshot, duration);
    }
}
