using UnityEngine;
using System.Collections;

public class Ch3P4_SketchbookHandler : MonoBehaviour
{
    private Texture2D cursor;
    private Animator animator;

    public AudioClip sketchbookDrop;
    
    void Start ()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown ()
    {
        animator.SetTrigger("pickup");
        Ch3_P4_POC pm = FindObjectOfType<Ch3_P4_POC>();
        if (sketchbookDrop)
            pm.GetComponent<AudioSource>().PlayOneShot(sketchbookDrop);
        pm.Invoke("SetSketcbookSolved", 1.3f);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
