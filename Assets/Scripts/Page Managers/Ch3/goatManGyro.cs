using UnityEngine;
using System.Collections;
using Fungus;
using UnityEngine.SceneManagement;

public class goatManGyro : MonoBehaviour {
    public float KBInputMultiplier = 0.5f;
    public float frameRateMultiplier = 30;
	public float disableGyroMoveMultiplier = 0.01f;
	public Flowchart flowchart;
//	public AudioSource steppedOnPanel;
//	public AudioSource skullInstalled;
	public AudioClip steppedOnPanel;
	public AudioSource skullInstalled;
	public AudioSource SFX;

    private float movementScale=8f;
	//increase tilt device range. tilt device more. 
	private float orginY = 3.87f;
	private float orginX = 0f;
	private float radius = 6.7f;
	private float detectRadius = 6f;
	//within 5f, enable goatMan Moving. enable real Gyro on screen.
	public float distance;
	public GameObject redSector;
	public GameObject brownSector;
	public GameObject blueSector;
	public GameObject greenSector;
	public GameObject skull;
	public bool enableGyro=false;
	private bool startDetect=false;
	private bool calledByFungusToDetect=false;
	//bool for start detecting device horizontal position 

	private float horizontalYMax = 4.2f;
	private float horizontalYMin = 3f;
	private float greenXMin = -5f;
	private float greenXMax = -3.4f;
	private float blueXMin = 3.6f;
	private float blueXMax = 5.2f;
	private float verticalXMin = -0.64f;
	private float verticalXMax = 1.2f;
	private float redYMin = 7.44f;
	private float redYMax = 9f;
	private float brownYMax = 0.3f;
	private float brownYMin = -1f;

	private float skullMoveUnit = 0.02f;

	//the area where skull can be moved 
	private float skullRangeXMin = -6.14f;
	private float skullRangeXMax = 6.03f;
	private float skullRangeYMin = -8.39f;
	private float skullRangeYMax = -3.88f;

	//the final position the skull should be installed 
	private float skullX = 2.83f;
	private float skullY = -3.88f;
	private float skullDelta = 0.3f;

	private bool skullSet = false;

	private Vector2 orgin;

	void Start () {
		 orgin = new Vector2 (orginX, orginY);

		redSector.SetActive(false);
		brownSector.SetActive(false);
		blueSector.SetActive(false);
		greenSector.SetActive(false);	
	}
	
	void Update() {
		Gyro ();
		activateSector ();
		skullRefine ();
		if (!skullSet) {
			skullInstall ();
		}
		detectGyro ();
		//tiltSB ();
	}


	void Gyro(){
		if (enableGyro) {
			Vector3 pos = transform.position;

			// Default - iOS
			float y = Vector3.Dot (Input.gyro.gravity, Vector3.up);
			float x = Vector3.Dot (Input.gyro.gravity, Vector3.right);


#if UNITY_ANDROID
            y = Input.acceleration.y;
            x = Input.acceleration.x;
#endif
#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR || UNITY_WEBPLAYER)
			y = Input.GetAxis ("Vertical") * KBInputMultiplier;
			x = Input.GetAxis ("Horizontal") * KBInputMultiplier;
#endif
			pos.y = y * movementScale + orginY;
			pos.x = x * movementScale + orginX;

			Vector2 posV2 = new Vector2 (pos.x, pos.y);

			//Vector2 posV2 = new Vector2(transform.position.x,transform.position.y);
			distance = Vector2.Distance (posV2, orgin);
			if (distance >= radius) {
				pos.x = (pos.x-orginX) * radius / distance+orginX;
				pos.y = (pos.y-orginY) * radius / distance+orginY;
				transform.localPosition = orgin;
				enableGyro = false;
				flowchart.SendFungusMessage("fadegoat");
				//send msg to flowchart, call fail function

				//StartCoroutine (wait ());
			} else{transform.position = pos;}

		} else {
			Vector3 pos = transform.position;


			float y = Input.GetAxis ("Vertical") * disableGyroMoveMultiplier;
			float x = Input.GetAxis ("Horizontal") * disableGyroMoveMultiplier;
			pos.y = y * movementScale + pos.y;
			pos.x = x * movementScale +  pos.x;

			Vector2 posV2 = new Vector2 (pos.x, pos.y);

			//Vector2 posV2 = new Vector2(transform.position.x,transform.position.y);
			distance = Vector2.Distance (posV2, orgin);
			if (distance >= radius) {
				pos.x = (pos.x - orginX) * radius / distance + orginX;
				pos.y = (pos.y - orginY) * radius / distance + orginY;
				//gameObject.SetActive (false);
				transform.localPosition = orgin;
				enableGyro = false;

				flowchart.SendFungusMessage ("fadegoat");
				//send msg to flowchart, call fail function
				//StartCoroutine (wait ());
			} else {
				transform.position = pos;
			}

		}

//		else {
//			Vector3 pos = transform.position;
//
//
//			float y = Input.GetAxisRaw ("Vertical") * KBInputMultiplier;
//			float x = Input.GetAxisRaw ("Horizontal") * KBInputMultiplier;
//
//			pos.y = y * movementScale + orginY;
//			pos.x = x * movementScale + orginX;
//
//			Vector2 posV2 = new Vector2 (pos.x, pos.y);
//
//			//Vector2 posV2 = new Vector2(transform.position.x,transform.position.y);
//			distance = Vector2.Distance (posV2, orgin);
//			if (distance < radius) {
//				transform.position = pos;
//			}
		//flowchart.SendFungusMessage("");
//		}
	}

	//called  after success. 
	public void disableGyro(){
		enableGyro = false;
	}

	//called by flowchart, after tapped on goatman.
	public void enableGyroByClick(){
		enableGyro = true;
	}
	IEnumerator wait()
	{
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void activateSector(){
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		//blue
		if (pos.x < blueXMax && pos.x > blueXMin && pos.y < horizontalYMax && pos.y > horizontalYMin) {
				blueSector.SetActive (true);
				moveSkull (4);

		}else {
			blueSector.SetActive(false);
			//Debug.Log ("bluefalse");
		}

		//green
		if (pos.x < greenXMax && pos.x > greenXMin && pos.y < horizontalYMax && pos.y > horizontalYMin) {
				greenSector.SetActive (true);
				moveSkull (3);

		}else {
			greenSector.SetActive(false);
			//Debug.Log ("bluefalse");

		}

		//red
		if (pos.y < redYMax && pos.y > redYMin && pos.x < verticalXMax && pos.x > verticalXMin) {
				redSector.SetActive (true);
				moveSkull (1);

		}else {
			redSector.SetActive(false);
			//Debug.Log ("bluefalse");

		}

		//brown
		if (pos.y < brownYMax && pos.y > brownYMin && pos.x < verticalXMax && pos.x > verticalXMin) {
				brownSector.SetActive (true);
				moveSkull (2);

		}else {
			brownSector.SetActive(false);
			//Debug.Log ("bluefalse");

		}

		if (!redSector.activeSelf && !brownSector.activeSelf && !blueSector.activeSelf && !greenSector.activeSelf) {			
			//steppedOnPanel.Stop ();
			SFX.Stop();
			//steppedOnPanel.
		} else if (skullSet) {
			//steppedOnPanel.Stop ();
			SFX.Stop();
		}
	}

	//dir means direction, 1 for up, 2 for down, 3 for left, 4 for right
	void moveSkull(int dir){

		if (!SFX.isPlaying) {
			//steppedOnPanel.Play ();
			SFX.PlayOneShot (steppedOnPanel);
		}

		if (!redSector.activeSelf && !brownSector.activeSelf && !blueSector.activeSelf && !greenSector.activeSelf) {			
			//steppedOnPanel.Stop ();
			SFX.Stop();
			//steppedOnPanel.
		} else if (skullSet) {
			//steppedOnPanel.Stop ();
			SFX.Stop();
		}
		//play sfx when steps on four panels 

		if (skullSet == false) {
			switch (dir) {
			case 1:
				skull.transform.position = skull.transform.position + new Vector3 (0, skullMoveUnit, 0) * Time.deltaTime * frameRateMultiplier;
			//up
				break;
			case 2:
				skull.transform.position = skull.transform.position - new Vector3 (0, skullMoveUnit, 0) * Time.deltaTime * frameRateMultiplier;
			//down
				break;
			case 3:
				skull.transform.position = skull.transform.position - new Vector3 (skullMoveUnit, 0, 0) * Time.deltaTime * frameRateMultiplier;
			//left
				break;
			case 4:
				skull.transform.position = skull.transform.position + new Vector3 (skullMoveUnit, 0, 0) * Time.deltaTime * frameRateMultiplier;
			//right
				break;
			}

		}

		if (skull.transform.position.x > -0.85) {
			flowchart.SendFungusMessage ("showSB");
		}
	}

	void skullRefine (){

		Vector3 pos = new Vector3 (skull.transform.position.x, skull.transform.position.y, 0);

		//		if (pos.x > skullRangeXMin && pos.x < skullRangeXMax 
		//			&& pos.y > skullRangeYMin && pos.y < skullRangeYMax) {
		//		
		//		}
		if (pos.x < skullRangeXMin) {
			pos.x = skullRangeXMin;
		}
		if (pos.x > skullRangeXMax) {
			pos.x = skullRangeXMax;
		}
		if (pos.y < skullRangeYMin) {
			pos.y = skullRangeYMin;
		}
		if (pos.y > skullRangeYMax) {
			pos.y = skullRangeYMax;
		}
		skull.transform.position = pos;
	}

	void skullInstall(){
		Vector3 pos = new Vector3 (skull.transform.position.x, skull.transform.position.y, 0);
		if (pos.x > skullX - skullDelta && pos.x < skullX + skullDelta
			&& pos.y > skullY - skullDelta && pos.y > skullY - skullDelta) {
			pos = new Vector3 (skullX, skullY, 0);
			skull.transform.position=pos;
			skullSet=true;

			flowchart.SendFungusMessage ("removeTiltSB");
			//remove tilt SB from screen 

			disableGyro ();
//			skullInstalled.Play ();
			skullInstalled.Play();
			//if installed, play sfx

			Debug.Log ("set true");
			Ch3_P6 pm = FindObjectOfType<Ch3_P6> ();
			if (pm)
				pm.SetSolved ();
		}

	}

	//detect device position. if its within 5f radius, enable goatman to move with gyro on screen
	void detectGyro(){
		if (startDetect) {
			Vector3 pos = transform.position;

			// Default - iOS
			float y = Vector3.Dot (Input.gyro.gravity, Vector3.up);
			float x = Vector3.Dot (Input.gyro.gravity, Vector3.right);


			#if UNITY_ANDROID
			y = Input.acceleration.y;
			x = Input.acceleration.x;
			#endif
			#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR || UNITY_WEBPLAYER)
			y = Input.GetAxis ("Vertical") * KBInputMultiplier;
			x = Input.GetAxis ("Horizontal") * KBInputMultiplier;
			#endif
			pos.y = y * movementScale + orginY;
			pos.x = x * movementScale + orginX;

			Vector2 posV2 = new Vector2 (pos.x, pos.y);

			//Vector2 posV2 = new Vector2(transform.position.x,transform.position.y);
			distance = Vector2.Distance (posV2, orgin);

			if (distance <= detectRadius) {
				enableGyro = true;
				flowchart.SendFungusMessage ("removeHintBubble");
				startDetect = false;

				Debug.Log ("detected");
			} else {
				Debug.Log ("startDetecting");
			}
		}
	}

	//show SB about tilting the ipad 
	public void tiltSB(){
		float distance=Vector2.Distance (transform.position,orgin);
		if (enableGyro && distance <= 0.3f) {
			flowchart.SendFungusMessage ("showTiltSB");
		} else if (enableGyro && distance > 0.3f) {
			flowchart.SendFungusMessage ("removeTiltSB");

		}
	}


	public void startDetectByFungus(){
		if (!calledByFungusToDetect) {
			startDetect = true;
			calledByFungusToDetect = true;
			Debug.Log ("startDetectByFungus");
		}
	}
	//called by fungus 
}



