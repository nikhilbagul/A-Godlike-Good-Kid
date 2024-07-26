using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GestureRecognizer;

public class NPHGesture : MonoBehaviour, INextPageHandler
{
    // Editor variables
    [SerializeField]
    private string resultName = "Next", gestureGOName = "GestureRecognizer";

    private string errorText = "No Gesture Recognizer Exception: Are you sure it is a child of this gameobject?";
    private int swipeCount = 0;
    private bool debug = false;

    private GestureBehaviour gestureRecogniser;

    // Interface
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

    void Start ()
    {
        try
        {
            gestureRecogniser = transform.Find(gestureGOName).GetComponent<GestureBehaviour>();
        }
        catch (System.Exception)
        {
            Debug.Assert(false, errorText, this);
        }
        Debug.Assert(gestureRecogniser.gameObject.activeSelf, errorText, this);
    }

    void OnEnable()
    {
        GestureBehaviour.OnRecognition += OnGestureRecognition;
    }

    void OnDisable()
    {
        GestureBehaviour.OnRecognition -= OnGestureRecognition;
    }

    void OnGestureRecognition (Result result)
    {
        if (result.Name == resultName && result.Score >=  0.7f)
        {
            //Debug.Log("Next page attempt #" + ++swipeCount);
            Debugger.LogMessage("Next page attempt #" + ++swipeCount + ", precision: " + result.Score, this);
            GameManager.Instance.AttemptNext();
        }
    }
}
