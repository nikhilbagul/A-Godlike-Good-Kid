using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;
    public static Singleton Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake ()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this);
	}
}
