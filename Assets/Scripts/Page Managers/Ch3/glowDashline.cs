using UnityEngine;
using System.Collections;

public class glowDashline : MonoBehaviour {
	Color alpha ;
	private bool decrease = true;
	private bool start = false;
	public float delta = 0.005f;
	public float threshold = 0.4f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		alpha = GetComponent<SpriteRenderer> ().color;

		if (alpha.a > 0.2f) {
			start = true;
		}

		if (start) {
			if (alpha.a >= threshold) {
				decrease = true;
			}

			if (decrease) {
				alpha.a -= delta;
			}

			if (alpha.a <= 0f) {
				decrease = false;
			}


			if (!decrease) {
				alpha.a += delta;
			}
			GetComponent<SpriteRenderer> ().color = alpha;
		}
	}
}
