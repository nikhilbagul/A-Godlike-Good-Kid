using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    void OnEnable ()
    {
        CutscenePlayer.Callback += IncrementSceneCounter;
    }

    void OnDisable()
    {
        CutscenePlayer.Callback -= IncrementSceneCounter;
    }

    // This increments the "Scene" variable in the Flowchart every time a new "scene" is triggered.
    // Use the template Flowchart to set what happens on each scene, how many scenes in total, etc.
    void IncrementSceneCounter ()
    {
        Fungus.Flowchart flowchart = GetComponent<Fungus.Flowchart>();
        flowchart.SetIntegerVariable("Scene", flowchart.GetIntegerVariable("Scene") + 1);
        flowchart.SendFungusMessage("Next");
    }
}
