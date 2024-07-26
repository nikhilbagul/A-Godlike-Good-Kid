using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System;

public class CameraSaturation : MonoBehaviour
{
    public float lerpDuration = 0.5f;

    public void SetLerpDuration(float duration)
    {
        lerpDuration = duration;
    }

    public void Saturate()
    {
        StartCoroutine(_Saturate(true));
    }

    public void Desaturate()
    {
        StartCoroutine(_Saturate(false));
    }

    IEnumerator _Saturate(bool saturate)
    {
        float invertedDuration = 1 / lerpDuration;
        ColorCorrectionCurves camera = GetComponent<ColorCorrectionCurves>();
        Func<bool> condition;
        if (saturate)
            condition = () => camera.saturation < 1;
        else
            condition = () => camera.saturation > 0;

        while (camera && condition())
        {
            camera.saturation += Time.deltaTime * invertedDuration * ((saturate) ? 1 : -1);
            yield return null;
        }

        if (camera)
            camera.saturation = (saturate) ? 1 : 0;
    }
}
