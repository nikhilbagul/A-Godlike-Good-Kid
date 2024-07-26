using UnityEngine;
using System.Collections;

public class Ch2_P5_OutroAnimation : MonoBehaviour
{
    public bool animate = false;
    public AudioClip bookFall;

    private bool animated = false;
    private Animator animator;
    private AudioSource audioSource;

    void Start ()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

	void Update ()
    {
        if (!animated)
        {
            if (animate)
            {
                if (bookFall)
                    audioSource.PlayOneShot(bookFall);
                animator.SetTrigger("shouldPlayAnimation");
                animated = true;
            }
        }
	
	}

    public void setAnimate()
    {
        animate = true;
    }
}
