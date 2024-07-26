using UnityEngine;
using System.Collections;

public static class CheckpointManager
{
    public static void CreateNewCheckPoint()
    {
        SaveData _saveData = Persistor.Load();
        _saveData.levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Persistor.Save(_saveData);

    }   
    
    public static void LoadSceneFromCheckpoint()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Persistor.Load().levelName);
    }

    public static void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
