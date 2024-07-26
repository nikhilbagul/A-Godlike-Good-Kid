using GestureRecognizer;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fungus;

public class Ch3P1 : PageManager {
    /*public GameObject Panel1;
	public GameObject Panel2;
	public GameObject Panel3;
	public GameObject Panel4;*/
	public GameObject[] borderDashLines;
	public GameObject[] gestureRecognizer;
	public GameObject[] sevenItems;
	public GameObject[] SpeechBubbles;


    public Flowchart flowchart;
    bool hasActivated = false, couldDrawDoor = false;
	private int countClickedItems = 0;

    public Animator[] interactables;
	public AudioClip[] pencil;
	public AudioSource SFX;
	//int mouseCount=0;

	protected override void Start () {
        base.Start();
        borderDashLines[6].SetActive(false);
	}

	void Update () {

		/*if (Input.GetMouseButtonUp (0)&&mouseCount==0) {
			Panel1.SetActive(true);
			mouseCount = 1;
		}
		else if (Input.GetMouseButtonUp (0)&&mouseCount==1) {
			Panel2.SetActive(true);
			mouseCount = 2;
		}
		else if (Input.GetMouseButtonUp (0)&&mouseCount==2) {
			Panel3.SetActive(true);
			mouseCount = 3;
		}
		else if (Input.GetMouseButtonUp (0)&&mouseCount==3) {
			Panel4.SetActive(true);
			mouseCount = 4;
		}
		else if (Input.GetMouseButtonUp (0)&&mouseCount==4) {
			for (int i = 0; i < borderDashLines.Length-1; i++) {
				borderDashLines [i].SetActive (true);
			}

			for (int i = 0; i < gestureRecognizer.Length-1; i++) {
				gestureRecognizer [i].SetActive (true);
			}

			mouseCount = 5;

		}*/

	}

	void OnGestureRecognition(Result r) {

		if (r.Score >= 0.75f) {
			Debug.Log (r.Name);
			SFX.PlayOneShot (pencil[Random.Range(0,pencil.Length)]);

			switch (r.Name) {
			case "Bookshelf":
				borderDashLines [0].SetActive (false);
				gestureRecognizer [0].SetActive (false);
				sevenItems [0].SetActive (true);
				disableOtherSpeechBubbles ();
				SpeechBubbles [0].SetActive (true);

				break;
			case "Chair":
				borderDashLines [1].SetActive (false);
				gestureRecognizer [1].SetActive (false);
				sevenItems [1].SetActive (true);
				break;
			case "WritingTable":
				borderDashLines [2].SetActive (false);
				gestureRecognizer [2].SetActive (false);
				sevenItems [2].SetActive (true);
                couldDrawDoor = true;
				disableOtherSpeechBubbles ();

				break;
			case "Drawing":
				borderDashLines [3].SetActive (false);
				gestureRecognizer [3].SetActive (false);
				sevenItems [3].SetActive (true);
				disableOtherSpeechBubbles ();
				SpeechBubbles [2].SetActive (true);
				break;
			case "Lamp":
				borderDashLines [4].SetActive (false);
				gestureRecognizer [4].SetActive (false);
				sevenItems [4].SetActive (true);
				break;
			case "Shoe":
				borderDashLines [5].SetActive (false);
				gestureRecognizer [5].SetActive (false);
				sevenItems [5].SetActive (true);
				break;

			}

			//countClickedItems += 1;

			if (couldDrawDoor&&hasActivated == false) {
//                borderDashLines [6].SetActive (true);
//                gestureRecognizer [6].SetActive (true);
               
                hasActivated = true;
			}

			if (r.Name=="Door") {
                flowchart.SendFungusMessage("showDoor");
                             
                borderDashLines [6].SetActive (false);
				gestureRecognizer [6].SetActive (false);
				disableOtherSpeechBubbles ();
				SpeechBubbles [1].SetActive (true);
                sevenItems[6].GetComponent<Clickable2D>().clickEnabled = true;
                //sevenItems [6].SetActive (true);*/
            }
		}
	}

	void disableOtherSpeechBubbles(){
		for (int i = 0; i < 3; i++) {
			SpeechBubbles [i].SetActive (false);
		}
	}

	void OnEnable() {
		GestureBehaviour.OnRecognition += OnGestureRecognition;
	}
	void OnDisable() {
		GestureBehaviour.OnRecognition -= OnGestureRecognition;
	}
	void OnDestroy() {
		GestureBehaviour.OnRecognition -= OnGestureRecognition;
	}

    public void SetInteractable(int arrayIndex)
    {
        interactables[arrayIndex].SetTrigger("enable");
    }

    public void FreezeInteractable(int arrayIndex)
    {
        interactables[arrayIndex].SetTrigger("freeze_t");
    }

    public void FreezeInteractableInstantly(int arrayIndex)
    {
        interactables[arrayIndex].SetTrigger("freeze");
    }

    public void SelectInteractable(int arrayIndex)
    {
        interactables[arrayIndex].SetTrigger("select");
    }

}
