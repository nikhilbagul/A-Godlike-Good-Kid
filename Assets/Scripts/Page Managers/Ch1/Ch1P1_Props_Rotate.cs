using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Fungus;
public class Ch1P1_Props_Rotate : MonoBehaviour {

	public float yThreshold = -2.4f;
	private float xThreshold = -1.2f;  //special for rotating salt;
	private float totalDistance; //distance from original position to activate area. only y direction 
	private float totalDistanceSalt; 
	private float currentDistance;
	private float ratio; // current/total
	private float startY;
	public Vector3 angle = new Vector3 (0,0,75f);
	private Vector3 origin;//copy original postion in the begining 
	public Color32 glowColor = new Color32(190,214,159,255); //color when being dragged 
	private bool glow;
	private int orderInLayer;
	public float originRange=0.1f;
	public bool rotateMovingLeft = false;//special for rotating salt;
	public bool isSalt = false;
	public Flowchart flowchart;
	private bool addedSalt = false; //add salt into cup;

	void Start () {
		startY = transform.localPosition.y;
		totalDistance =	transform.localPosition.y - yThreshold;
		totalDistanceSalt=	transform.localPosition.x - xThreshold;
		origin = transform.localPosition;
		glow = false;
		orderInLayer = GetComponent<SpriteRenderer> ().sortingOrder;
	}
	
	void Update () {
		rotateProp ();
		changeColor (); 
	}

	void rotateProp(){
		if (!rotateMovingLeft &&  transform.localPosition.y  <   origin.y -originRange  )
		{
			currentDistance = startY-	transform.localPosition.y ;
			ratio = currentDistance / totalDistance;

			if (transform.localPosition.y < yThreshold) {
				ratio = 1f;
			}

			transform.eulerAngles = ratio * angle;
		}

		else if (rotateMovingLeft &&  transform.localPosition.x  <   origin.x -originRange) {
			currentDistance = origin.x -	transform.localPosition.x ;
			ratio = currentDistance / totalDistanceSalt;
			if (transform.localPosition.x < xThreshold) {
				ratio = 1f;
			}
			transform.eulerAngles = ratio * angle;
		}
//		else if (Mathf.Abs (transform.localPosition.x - origin.x) < originRange
//		    && Mathf.Abs (transform.localPosition.y - origin.y) < originRange) 

		//Mathf.Abs( transform.localPosition.x - origin.x )>originRange &&
		else {
			transform.eulerAngles = 0 * angle;

		}

	}

	void restartLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		//this fucntion is called in flowchart, if player drags the wrong props;
	}

	void changeColor(){
		if (!isSalt) {
			if (Mathf.Abs (transform.localPosition.x - origin.x) > originRange
			    && Mathf.Abs (transform.localPosition.y - origin.y) > originRange
			    && !glow) {
				GetComponent<SpriteRenderer> ().sortingOrder += 5;
				GetComponent<SpriteRenderer> ().color = glowColor;
				glow = true;

			}

			if (Mathf.Abs (transform.localPosition.x - origin.x) < originRange
			    && Mathf.Abs (transform.localPosition.y - origin.y) < originRange
			    && glow) {
				glow = false;
				GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
				GetComponent<SpriteRenderer> ().sortingOrder = orderInLayer;

			}
		} else {
			if (Mathf.Abs (transform.localPosition.x - origin.x) > originRange
				&& Mathf.Abs (transform.localPosition.y - origin.y) > originRange
				&& !glow) {
				GetComponent<SpriteRenderer> ().sortingOrder += 5;
				glow = true;

			}

			if (Mathf.Abs (transform.localPosition.x - origin.x) < originRange
				&& Mathf.Abs (transform.localPosition.y - origin.y) < originRange
				&& glow) {
				glow = false;
				GetComponent<SpriteRenderer> ().sortingOrder = orderInLayer;
			}
		}
		//change color when being dragged, also change layer order
	}

	//only for salt. set cup constant glow after drag salt onto cup. called by flowchart;
	public void setToConstantGlow(){
		isSalt = true;
	}

	//only for cup with/whithout salt; called by flowchart;
	public void cupPour(){
		if (addedSalt) {
			//int count = 0;
			//while(flowchart.GetIntegerVariable ("Finish")==count){
				flowchart.SendFungusMessage ("cupWithSalt");
				//flowchart.SetIntegerVariable("Finish", flowchart.GetIntegerVariable("Finish") + 1);
				//count += 1;
			//}
		} else {
			flowchart.SendFungusMessage ("cupWithoutSalt");
		}
	}

	//if add salt into cup, call this function; called by flowchart;
	public void addSalt(){
		addedSalt = true;
	}
}
