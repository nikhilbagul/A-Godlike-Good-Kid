using UnityEngine;
using System.Collections;
using System;

public class Fader : MonoBehaviour
{
    public static float _fadeDuration;

    [SerializeField] [Range (0.1f, 3.0f)]
    private float fadeDuration = 2f;

    private bool fading = false;
    private Color opaque = new Color(1, 1, 1, 0), transparent = new Color(1, 1, 1, 1);
    private float increment;

    void Start()
    {
        increment = 1 / fadeDuration;
        _fadeDuration = fadeDuration;
    }


    public void FadePageOut() { FadeIn(); }
    public void FadeIn(SpriteRenderer spr = null, CanvasGroup cvg = null)
    {
        if (!cvg)
        {
            if (!spr)
                spr = GetComponent<SpriteRenderer>();

            spr.color = opaque;
            if (spr)
                StartCoroutine(Fade(() => spr.color.a < 1, spr, true));
        }
        else
        {
            cvg.alpha = 0;
            StartCoroutine(Fade(() => cvg.alpha < 1, cvg, true));
        }

        fading = true;
    }

    public void FadePageIn() { FadeOut(); }
    public void FadeOut(SpriteRenderer spr = null, CanvasGroup cvg = null)
    {
        if (!cvg)
        {
            if (!spr)
                spr = GetComponent<SpriteRenderer>();
         
            spr.color = transparent;
            if (spr)
                StartCoroutine(Fade(() => spr.color.a > 0, spr, false));
        }

        else
        {
            cvg.alpha = 1;
            StartCoroutine(Fade(() => cvg.alpha > 0, cvg, false));
        }
        fading = true;
    }

    public bool IsFadeComplete()
    {
        return !fading;
    }

    public float GetFadeDuration()
    {
        return fadeDuration;
    }

    IEnumerator Fade(Func<bool> condition, SpriteRenderer spr, bool makeOpaque)
    {
        Color c = spr.color;
        while (spr && condition())
        {
            float a = spr.color.a + increment * Time.deltaTime * ((makeOpaque) ? 1 : -1);
            spr.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        fading = false;
    }

    IEnumerator Fade(Func<bool> condition, CanvasGroup cvg, bool makeOpaque)
    {
        while (cvg && condition())
        {
            cvg.alpha = cvg.alpha + increment * Time.deltaTime * ((makeOpaque) ? 1 : -1);
            yield return null;
        }
        fading = false;
    }
}
