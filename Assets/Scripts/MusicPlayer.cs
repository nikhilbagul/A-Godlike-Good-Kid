using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public float fadeDuration = 3;

    [Header("AUTOFILL")] [Tooltip("Put Audiosources as children of this object, not as references here!")]
    public AudioSource[] overlays;

    private static MusicPlayer instance;
    public static MusicPlayer Instance { get { return instance; } }

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        overlays = GetComponentsInChildren<AudioSource>();
    }

    public bool FadeOverlay(int overlayNumber, float toVolume=1)
    {
        if (overlays != null && overlays[overlayNumber] != null)
        {
            StartCoroutine(FadeIn(overlayNumber, toVolume));
            return true;
        }
        return false;
    }

    IEnumerator FadeIn(int arrayIndex, float toVolume)
    {
        float invFadeDuration = 1 / fadeDuration;
        while (overlays[arrayIndex].volume < toVolume)
        {
            overlays[arrayIndex].volume += Time.deltaTime * invFadeDuration;
            yield return null;
        }
        overlays[arrayIndex].volume = toVolume;
    }
}
