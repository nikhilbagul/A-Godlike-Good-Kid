using UnityEngine;
using System.Collections;

public class Ch3_P6_goatManFlash : MonoBehaviour {

	Color alpha ;
	private bool decrease = true;
	private bool start = false;
	public float delta = 0.015f;
	public float threshold = 1f;
	public float lowerThreshold = 0.3f;
	private bool tapToStopFlash= false;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (!tapToStopFlash) {
			alpha = GetComponent<SpriteRenderer> ().color;

			if (alpha.a > 0.2f) {
				start = true;
			} else {
				start = false;
			}

			if (start) {
				if (alpha.a >= threshold) {
					decrease = true;
				}

				if (decrease) {
					alpha.a -= delta;
				}

				if (alpha.a <= lowerThreshold) {
					decrease = false;
				}


				if (!decrease) {
					alpha.a += delta;
				}
				GetComponent<SpriteRenderer> ().color = alpha;
			}
		}
	}

	public void tapToStop(){
		tapToStopFlash = true;
		alpha.a = 1f;
		GetComponent<SpriteRenderer> ().color = alpha;

	}
}
