using UnityEngine;
using System.Collections;

public class Ch3_P6 : PageManager
{
	public static int numbOfFails;




	void Awake()
	{
		FindObjectOfType<Fungus.Flowchart>().SetIntegerVariable("numbOfFails", numbOfFails);
	}


    protected override void Start()
    {
        base.Start();
		FindObjectOfType<Fungus.Flowchart>().SetIntegerVariable("numbOfFails", numbOfFails);

        MusicPlayer mp = FindObjectOfType<MusicPlayer>();
        if (mp)
            mp.FadeOverlay(3);
    }

	public	void	fail(){
		numbOfFails+=1;
		//FindObjectOfType<Fungus.Flowchart>().SetIntegerVariable("numbOfFails", numbOfFails);
		SetSolved (advance: false);

	}





	public void win(){
		SetSolved (advance: true);
	}




}
