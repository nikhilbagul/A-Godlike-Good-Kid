using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Ch3_P8_Manager : PageManager
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
}


