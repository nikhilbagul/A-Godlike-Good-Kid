using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Ch3P9_Manager : PageManager
{

    public GameObject mainCamera;

    public void camera_saturation()
    {
        mainCamera.GetComponent<ColorCorrectionCurves>().enabled = true;
    }

    public void callNextPage()
    {
        // Page has been completed, then just call:
        SetSolved();
    }


    public void PlayOverlay5()
    {
        MusicPlayer mp = FindObjectOfType<MusicPlayer>();
        if (mp)
            mp.FadeOverlay(5, 0.7f);
    }
}


