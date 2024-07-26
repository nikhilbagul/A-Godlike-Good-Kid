using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebugButtonHandler : MonoBehaviour
{
    private float buttonPressTimeout;
    private Slider debugSlider;
    private List<Button> buttons = new List<Button>();
    private short buttonsPressed;
    private bool countingDown = false;
    private float elapsed = 0;

    void Start ()
    {
        GetButtons();
        ResetButtons();
        buttonPressTimeout = FindObjectOfType<Debugger>().buttonPressTimeout;
    }

    void GetButtons()
    {
        for (int i = 0; i < transform.childCount; ++i)
            buttons.Add(transform.GetChild(i).GetComponent<Button>());
    }

    void ResetButtons (bool show = false)
    {
        buttonsPressed = 0;
        elapsed = 0;
        countingDown = false;
        buttons[0].enabled = true;
        for (int i = 1; i < buttons.Count; ++i)
            buttons[i].enabled = false;
    }
    
    public void OnClick ()
    {
        countingDown = true;
        ++buttonsPressed;
        Debugger.LogMessage("Debug button " + buttonsPressed + " pressed.");
        if (buttonsPressed >= buttons.Count)
        {
            ResetButtons();
            GameManager debugger = FindObjectOfType<GameManager>();
            debugger.debug = !debugger.debug;
        }
        else
        {
            buttons[buttonsPressed - 1].enabled = false;
            buttons[buttonsPressed].enabled = true;
        }
    }

    public void Update ()
    {
        CountdownTimer();
    }

    void CountdownTimer()
    {
        if (countingDown)
        {
            elapsed += Time.deltaTime;
            
            if (elapsed >= buttonPressTimeout)
                ResetButtons();

            //Debug.Log("time left: " + (buttonPressTimeout - elapsed));     // working fine
        }
    }
}
