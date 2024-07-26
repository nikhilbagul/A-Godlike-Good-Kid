using UnityEngine;
using System.Collections;

public class CarryoverSFX : MonoBehaviour
{
    private static CarryoverSFX instance;
    public static CarryoverSFX Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake ()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
	}

    public void FadeOutAndDestroy()
    {
        StartCoroutine(_FadeOutAndDestroy());
    }

    IEnumerator _FadeOutAndDestroy()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        float secondsInverse = 1.4f;
        while (audioSource && audioSource.volume > 0)
        {
            audioSource.volume -= (Time.deltaTime * secondsInverse);
            Debugger.LogMessage(audioSource.volume.ToString(), this);
            yield return null;
        }
        if (this)
        {
            if (audioSource)
                audioSource.volume = 0;
            Destroy(gameObject);
        }
    }
}
