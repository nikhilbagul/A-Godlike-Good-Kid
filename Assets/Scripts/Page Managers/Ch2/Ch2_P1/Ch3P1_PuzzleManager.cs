using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.ImageEffects;

public class Ch3P1_PuzzleManager : PageManager
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
