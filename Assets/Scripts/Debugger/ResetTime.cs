using UnityEngine;
using System.Collections;

public class ResetTime : MonoBehaviour
{
    bool tapped = false;
    float timeout = 0.5f, elapesd = 0;

    void OnMouseDown ()
    {
        if (!tapped)
            tapped = true;
        else
            FindObjectOfType<Debugger>().ResetTimeScale();
    }

    void Update ()
    {
        if ((elapesd += Time.unscaledDeltaTime) > timeout)
            tapped = false;
    }
}
