using UnityEngine;
using System.Collections;

public class Ch2_P3 : PageManager
{ 
    public void StartFireSFX()
    {
        StartCoroutine(RampUpVolume());
    }

    IEnumerator RampUpVolume()
    {
        AudioSource carryoverSFX = FindObjectOfType<CarryoverSFX>().GetComponent<AudioSource>();
        carryoverSFX.Play();

        while (carryoverSFX.volume < 1)
        {
            carryoverSFX.volume += Time.deltaTime;
            yield return null;
        }
    }
}
