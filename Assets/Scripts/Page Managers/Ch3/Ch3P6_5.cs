using UnityEngine;
using System.Collections;

public class Ch3P6_5 : PageManager
{
    private Typewriter typer;

    protected override void Start ()
    {
        typer = GetComponent<Typewriter>();
        FindObjectOfType<SnapshotSwitcher>().Switch();
        base.Start();
    }

	void Update ()
    {
        if (typer.state == Typewriter.TypeState.completed && !IsSolved)
            SetSolved();
	}
}
