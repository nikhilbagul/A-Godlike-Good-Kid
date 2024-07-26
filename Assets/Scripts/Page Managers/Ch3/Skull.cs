using UnityEngine;
using System.Collections;
using Fungus;

public class Skull : MonoBehaviour
{
    
    public Flowchart flowchart;

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.name == "Destination")
        {
            flowchart.SendFungusMessage("hasReached!");
            FindObjectOfType<Ch3_P2>().SetSolvedAfterDelay(2);
            col.GetComponent<Collider2D>().enabled = false;
            //GetComponent<Draggable2D>().enabled = false;
            GetComponent<Clickable2D>().enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D m)
    {
        
        if(m.gameObject.name == "Map")
        {
            if(!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();
            FindObjectOfType<SnapshotSwitcher>().Switch();
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        GetComponent<AudioSource>().Stop();
    }

    public void disableDrag()
    {
        //GetComponent<Draggable2D>().enabled = false;
        GetComponent<Clickable2D>().enabled = false;
    }
}
