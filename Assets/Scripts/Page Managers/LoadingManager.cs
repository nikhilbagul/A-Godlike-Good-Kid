using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class LoadingManager : MonoBehaviour
{

    public CanvasGroup loadingScreenCanvas;

    private string fileName = "data.sav";
    string path = "";

    GameObject loadButton;
    void Start()
    {
        loadButton = GameObject.Find("load game button");
        loadButton.GetComponent<Button>().interactable = false;

        //get current guid
        SaveData data = Persistor.Load();


#if !UNITY_EDITOR
        path = Application.persistentDataPath + "/";
        //userDataPath = Application.persistentDataPath + "/";
#endif
        path += fileName;
        if (File.Exists(path) && Persistor.Load().levelName != "NULL")
            loadButton.GetComponent<Button>().interactable = true;

        StartCoroutine(DelayedShowCanvas());
    } 

    IEnumerator DelayedShowCanvas()
    {
        loadingScreenCanvas.alpha = 0;
        float elapsed = 0;
        while (elapsed < 1)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0;
        while (elapsed < 1)
        {
            elapsed += Time.deltaTime;
            loadingScreenCanvas.alpha += Time.deltaTime;
            yield return null;
        }
        loadingScreenCanvas.alpha = 1;
    }

   public void LoadFromCheckpoint()
    {
        GameManager.Instance.UnsetCamera();
        CheckpointManager.LoadSceneFromCheckpoint();
    }

   public void NewGame()
    {

        //send data
        //DataPoster.instance.SendUserData(new_game_url + GUID + replay_times.ToString() + System.DateTime.Now.ToString());
        GMPersistor.Instance.SetupGUID();

#if UNITY_EDITOR
        URLSetter.NewGameCount(true);
#else
        URLSetter.NewGameCount();
#endif

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        GameObject.Find("New game button").GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(0.5f);
        CheckpointManager.NewGame();
    }
}
