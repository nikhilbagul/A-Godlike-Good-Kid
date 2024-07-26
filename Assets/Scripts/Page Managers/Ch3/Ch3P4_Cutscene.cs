using UnityEngine;
using System.Collections;

public class Ch3P4_Cutscene : MonoBehaviour
{
    public float increment = 1;
    public bool fading = false;

    public IEnumerator FadeIn(SpriteRenderer spr)
    {
        fading = true;
        Color c = spr.color;
        while (spr.color.a  < 1)
        {
            float a = spr.color.a + increment * Time.deltaTime;
            spr.color = new Color(c.r, c.g, c.b, a);
            yield return new WaitForEndOfFrame();
        }
        fading = false;
    }

    public IEnumerator FadeOut(SpriteRenderer spr)
    {
        fading = true;
        Color c = spr.color;
        while (spr.color.a > 0)
        {
            float a = spr.color.a - increment * Time.deltaTime;
            spr.color = new Color(c.r, c.g, c.b, a);
            yield return new WaitForEndOfFrame();
        }
        fading = false;
    }
}
