using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class GUIDAlphabet
{
    public static char[] alphabet = {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        };
}

[System.Serializable]
public class SaveData
{    
    string GUID;
    public string GetGUID { get { return GUID; } }
    public string levelName;

    public SaveData()
    {
        GUID = "NOT_INITIALISED";
        levelName = "NULL";
    }

    public void GenerateGUID()
    {
        string newGUID = "";
        for (int i = 0; i < 8; ++i)
        {
            char next = GUIDAlphabet.alphabet[Random.Range(0, GUIDAlphabet.alphabet.Length)];
            newGUID += next;
        }

        Debug.Log("Generated GUID = " + newGUID);
        GUID = newGUID;
    }

    public override string ToString()
    {
        return "GUID: " + GUID + ", scene: " + levelName;
    }
}

public static class Persistor
{
    static string fileName = "data.sav";

    public static SaveData Load()
    {
        string path = "";

#if !UNITY_EDITOR
        path = Application.persistentDataPath + "/";
#endif
        path += fileName;
        Debug.Log(path);

        SaveData saveData = new SaveData();
        FileStream saveFile;

        if (File.Exists(path))
        {
            saveFile = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            saveData = (SaveData)formatter.Deserialize(saveFile);
            saveFile.Close();
        }
        return saveData;
    }

    public static void Save(SaveData saveData)
    {
        string path = "";

#if !UNITY_EDITOR
        path = Application.persistentDataPath + "/";
#endif
        path += fileName;

        FileStream saveFile = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(saveFile, saveData);
            Debugger.LogMessage(saveData + " saved.");
        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            saveFile.Close();
        }
    }

    public static void Delete()
    {
        string path = "";

#if !UNITY_EDITOR
        path = Application.persistentDataPath + "/";
#endif
        path += fileName;

        if (File.Exists(path))
        {
            Debugger.LogMessage("Save file found and deleted.");
            File.Delete(path);
        }
        else
            Debugger.LogMessage("Save file not found.");

    }

    public static void ObliterateCheckpoint()
    {
        SaveData save = Load();
        save.levelName = "NULL";
        Save(save);
    }
}
