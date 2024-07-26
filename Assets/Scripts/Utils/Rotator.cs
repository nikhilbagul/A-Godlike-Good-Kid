using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public enum Direction { Clockwise=-1, Counter=1 };
    public Direction direction;

    public float speed = 1;
	
	void Update ()
    {
        transform.Rotate(Vector3.forward, (int)direction * Time.deltaTime * 100);
	}
}
