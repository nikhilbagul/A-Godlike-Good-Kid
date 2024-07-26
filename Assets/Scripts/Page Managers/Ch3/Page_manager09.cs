using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Page_manager09 : PageManager
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
