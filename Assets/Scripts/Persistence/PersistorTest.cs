using UnityEngine;
using System.Collections;

public class PersistorTest : MonoBehaviour {

	void Start ()
    {
        SaveData uninitialised = new SaveData();
        SaveData saveData = Persistor.Load();

        // Create new GUID
        if (saveData.GetGUID.Equals(uninitialised.GetGUID))
        {
            uninitialised.GenerateGUID();
            Persistor.Save(uninitialised);
        }

        // Use encoded GUID
        else
        {
            Debug.Log("GUID exists: " + saveData.GetGUID);
        }

        
	}
}
