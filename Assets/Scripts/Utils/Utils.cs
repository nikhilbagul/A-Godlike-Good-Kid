using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour
{
    public delegate void Void();
    public delegate void VoidBool(bool b);
    public delegate void VoidInt(int i);
    public delegate void VoidFloat(float f);
    public delegate void VoidString(string x);

    [System.Serializable]
    public struct clamp { public float xMin, xMax, yMin, yMax; }
}
