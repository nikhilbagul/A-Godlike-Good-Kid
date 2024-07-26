using UnityEngine;
using System.Collections;

public class Ch1_P1 : PageManager {
	public static int numbOfFails;

	public	void	fail(){
		numbOfFails+=1;
		//FindObjectOfType<Fungus.Flowchart>().SetIntegerVariable("numbOfFails", numbOfFails);
		SetSolved (advance: false);

	}

	void Awake()
	{
		FindObjectOfType<Fungus.Flowchart>().SetIntegerVariable("numbOfFails", numbOfFails);
	}

	protected override void Start()
	{
		base.Start();
		FindObjectOfType<Fungus.Flowchart>().SetIntegerVariable("numbOfFails", numbOfFails);
		 
		Debug.Log ("numbOfFails: " + numbOfFails);
	}

	void Update()
	{
		//numbOfFails = (State)cutscenePlayer.GetIntegerVariable("State");
	}

	public void win(){
		SetSolved (advance: true);
	}
}
