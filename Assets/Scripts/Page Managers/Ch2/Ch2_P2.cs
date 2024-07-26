using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Fungus;

public class Ch2_P2 : PageManager {
    public Slider slider;
    public Flowchart flowchart;
    float value;
    Clickable2D pageCollider;
    // DEBUG ONLY
    int _prev_state;
    // DEBUG ONLY

    public enum progreeBarState { first, second, third, forth, fifth };
    progreeBarState state;
    progreeBarState previousState;

    // Use this for initialization
   protected override void Start () {
        base.Start();
        state = progreeBarState.first;
        previousState = state;
    }

	// Update is called once per frame
	void Update () {
        value = slider.value;
        previousState = state;

        int _state = (int) (value / 0.2f);
        //if (_state != _prev_state)
            //Debug.Log(_state);

        state = (progreeBarState)_state;
        //Debug.Log(state);
        //_prev_state = _state;

     
        if(state != previousState)
        {
           //Debug.Log(state);           
           switch (state)
            {
                case progreeBarState.first:
                    flowchart.SendFungusMessage("box");
                    flowchart.SetIntegerVariable("States", 1);
                    break;
                case progreeBarState.second:
                    flowchart.SendFungusMessage("carpet");
                    flowchart.SetIntegerVariable("States", 2);

                    break;
                case progreeBarState.third:
                    flowchart.SendFungusMessage("couch");
                    flowchart.SetIntegerVariable("States", 3);

                    break;
                case progreeBarState.forth:                    
                    flowchart.SendFungusMessage("book");
                    flowchart.SetIntegerVariable("States", 4);

                    break;
                case progreeBarState.fifth:
                    flowchart.SendFungusMessage("door");
                    flowchart.SetIntegerVariable("States", 5);

                    break;
            }
                
        }
        
	}

    public void hideWhenDragging()
    {
        flowchart.SendFungusMessage("hide_result_sprites");
    }

    void disableSlider() 
    {
        slider.enabled = false;
    }

    void OnEnable()
    {
        GameManager.TurnPage += disableSlider;
        
    }

    void OnDisable()
    {
        GameManager.TurnPage -= disableSlider;
    }

}

