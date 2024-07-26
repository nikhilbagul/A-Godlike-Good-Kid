using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public abstract class PageManager : MonoBehaviour
{
    protected bool isSolved = false;

    private float solvedTime = 0, turnedTime = 0;
    
    Coroutine SolvedTimerHandle, TurnedTimerHandle;

    public bool IsSolved
    {
        get { return isSolved; }
    }

    protected virtual void Start ()
    {
        GameManager.Instance.ToggleNext(false);
        GameManager.Instance.InitScene();
        SetGameManagerReference();
        Debugger.LogMessage("Prefabs instantiated, references set.");
        CheckpointManager.CreateNewCheckPoint();

        // Start timer for page
        SolvedTimerHandle = StartCoroutine(SolvedTimer());
        
    }

    IEnumerator SolvedTimer()
    {
        while (true)
        {
            solvedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator TurnedTimer()
    {
        while (true)
        {
            turnedTime += Time.deltaTime;
            yield return null;
        }
    }


    protected bool SetGameManagerReference()
    {
        try
        {
            GameManager.currentPageManager = this;
            return true;
        }
        catch (System.Exception e)
        {
            Debugger.LogMessage(e.Message, this);
            return false;
        }
    }

    public virtual void SetSolved (bool advance=true)
    {
        Debug.Log(name + " is setting advance = " + advance);
        isSolved = advance;
        GameManager.Instance.ToggleNext(true);
        //send page time data
        //DataPoster.instance.SendUserData(solved_time_url+GUID+page_name+elapsed.ToString()+isSolved.ToString()+game_version+System.DateTime.Now.ToString());

        if (SolvedTimerHandle != null)
            StopCoroutine(SolvedTimerHandle);
        TurnedTimerHandle = StartCoroutine(TurnedTimer());
    }

    public virtual void LoadNextPage (bool advance=true)
    {
        if (TurnedTimerHandle != null)
            StopCoroutine(TurnedTimerHandle);

        bool isDevData = false;
#if UNITY_EDITOR
        isDevData = true;
#endif
        URLSetter.PageTime(SceneManager.GetActiveScene().name, solvedTime, turnedTime, GameManager.Instance.TotalTime, isSolved, isDevData);

        Debug.Log(name + " is loading next page via base class method. Advance = " + advance);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + (advance ? 1 : 0));
    }
}
