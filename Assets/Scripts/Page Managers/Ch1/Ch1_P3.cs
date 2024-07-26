using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Fungus;

public class Ch1_P3 : PageManager {
    public GameObject targets;
    public Flowchart flowchart;
    public static int gameCheckPoint = 0;
    public AudioSource music;

    void OnEnable()
    {
        GameManager.TurnPage += DestroyMusic;
    }

    void OnDisable()
    {
        GameManager.TurnPage -= DestroyMusic;
    }

    void Awake()
    {
        flowchart.SetIntegerVariable("checkPoint", gameCheckPoint);
    }

    protected override void Start()
    {
        base.Start();
        disableAll();
        if (music)
            StartCoroutine(FadeInMusic());
    }

    void Update()
    {

    }

    public void enableAll()
    {
        //Debug.Log("called!@");
        BoxCollider2D[] t = targets.GetComponentsInChildren<BoxCollider2D>();
        foreach(BoxCollider2D b in t)
        b.enabled = true;
    }

    public void disableAll()
    {
        
        BoxCollider2D[] t = targets.GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D b in t) 
            b.enabled = false;
    }

    public void reloadWithoutDisplay()
    {
        gameCheckPoint += 1;
    }

    IEnumerator FadeInMusic()
    {
        while (music && music.volume < 1)
        {
            music.volume += Time.deltaTime;
            yield return null;
        }
    }

    void DestroyMusic()
    {
        if (isSolved)
        {
            CarryoverSFX[] all = FindObjectsOfType<CarryoverSFX>();
            foreach(CarryoverSFX c in all)
                c.FadeOutAndDestroy();
        }
    }
   
}
