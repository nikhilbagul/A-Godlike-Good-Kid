using UnityEngine;

public class NPHManual : MonoBehaviour, INextPageHandler
{
    public bool verbose = true;
    public float minDistance;

    private Vector2 startPosition;
    private static int swipeCount;

    public void ToggleDogear(bool setVisible)
    {
        GameObject dogear = transform.Find("Dogear").gameObject;
        dogear.SetActive(setVisible);

        CanvasGroup cvg = dogear.GetComponent<CanvasGroup>();
        if (setVisible)
            FindObjectOfType<Fader>().FadeIn(null, cvg);
        else
            FindObjectOfType<Fader>().FadeOut(null, cvg);
    }

    public void SetFailed()
    {
        //GameObject dogear = transform.Find("Dogear").gameObject;
        //dogear.GetComponent<Animator>().SetTrigger("fail");
        //dogear.transform.Find("FailIcon").gameObject.SetActive(true);

        Transform dogearParent = transform.Find("Dogear");
        GameObject[] dogears = { dogearParent.GetChild(0).gameObject, dogearParent.GetChild(1).gameObject };
        foreach (GameObject dogear in dogears)
        {
            dogear.GetComponent<Animator>().SetTrigger("fail");
            dogear.transform.Find("FailIcon").gameObject.SetActive(true);
        }
        
    }

    void Start ()
    {
        
    }

    void Update()
    {
        // Handle Touches
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startPosition = Input.GetTouch(0).position;
            if (verbose)
                Debugger.LogMessage("Swipe started at " + startPosition.x + "," + startPosition.y);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 endPosition = Input.GetTouch(0).position;
            if(verbose)
                Debugger.LogMessage("Swipe ended at " + endPosition.x + "," + endPosition.y);
            HandleSwipe(endPosition);
        }

        // Handle clicks
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ConvertClickToTouch(Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began);
            }
            if (Input.GetMouseButton(0))
            {
                ConvertClickToTouch(Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0))
            {
                ConvertClickToTouch(Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended);
            }
        }
    }

    void ConvertClickToTouch(Vector2 position, TouchPhase touchPhase)
    {
        switch (touchPhase)
        {
            case TouchPhase.Began:
                startPosition = position;
                if (verbose)
                    Debugger.LogMessage("Swipe started at " + position.x + "," + position.y);
                break;

            case TouchPhase.Ended:
                Vector2 endPosition = position;
                HandleSwipe(endPosition);
                if(verbose)
                    Debugger.LogMessage("Swipe ended at " + position.x + "," + position.y);
                break;
        }
    }

    void HandleSwipe (Vector2 endPosition)
    {
        Vector2 delta = endPosition - startPosition;

        float x_dist = startPosition.x - endPosition.x;
        Debug.Log(x_dist);
        // Right to left
        if (x_dist >= minDistance)
        {
            if (verbose)
                Debugger.LogMessage("Next page attempt #" + ++swipeCount);
            AttemptNext();
        }

        startPosition = Vector2.zero;
    }

    public void AttemptNext()
    {
        GameManager.Instance.AttemptNext();
    }
}
