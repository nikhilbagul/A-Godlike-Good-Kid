using UnityEngine;
using System.Collections;

public class GMPersistor : MonoBehaviour
{
    private SaveData _saveData;
    public SaveData saveData { get { return _saveData; } }

    private static GMPersistor instance;
    public static GMPersistor Instance { get { return instance; } }

    void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetupGUID();
    }

	public void SetupGUID ()
    {
        _saveData = Persistor.Load();
        // Create new GUID
        if (_saveData.GetGUID.Equals(new SaveData().GetGUID))
        {
            _saveData.GenerateGUID();           
            Persistor.Save(_saveData);
        }

        // Use encoded GUID
        else
        {
            Debug.Log("GUID exists: " + _saveData.GetGUID);
            Debug.Log("Checkpoint exists: " + _saveData.levelName);
        }
    }

    public string GetGUID()
    {
        SetupGUID();
        return Persistor.Load().GetGUID;
    }
}
