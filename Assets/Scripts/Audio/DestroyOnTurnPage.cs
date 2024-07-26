using UnityEngine;
using System.Collections;

public class DestroyOnTurnPage : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.TurnPage += FadeOutAndDestroy;
    }

    void OnDisable()
    {
        GameManager.TurnPage -= FadeOutAndDestroy;
    }

    void FadeOutAndDestroy()
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
