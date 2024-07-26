using UnityEngine;
using System.Collections;

public class MovingPanel : MonoBehaviour
{ 
    // Puzzle manager should subscribe the swapping method to this class delegate
    public delegate void OnClick(GameObject obj);
    public static OnClick onClickDelegate;

    public enum State { Fixed, Moveable, Selected };

    public State initState;
    public Texture2D cursor;
    public Sprite solvedSprite;
    public AudioClip[] clickSounds;

    public class StateMachine
    {   
        public struct Triggers
        {
            public bool setMoveable, setFixed, select, setSolved;
        };
        public Triggers triggers;
        public State state;
    };
    public StateMachine stateMachine;

    private State prevState;
    private Animator animator;
    private SpriteRenderer spriteRend;
    private Sprite bufferSprite;
    private AudioSource audioSource;
	
	void Start ()
    {
        animator = GetComponent<Animator>();
        stateMachine = new StateMachine();
        stateMachine.state = initState;

        if (stateMachine.state == State.Moveable)
            StartCoroutine(DelayedFloat());
        if (stateMachine.state == State.Fixed)
            stateMachine.triggers.setFixed = true;

        audioSource = GetComponent<AudioSource>();
        spriteRend = GetComponent<SpriteRenderer>();
	}

    void OnEnable ()
    {
        Start();
    }
	
	void Update ()
    {
        // DEBUG
        initState = stateMachine.state;

        if (stateMachine.triggers.setMoveable)
        {
            setMoveable();
            stateMachine.triggers.setMoveable = false;
        }

        if (stateMachine.triggers.setFixed)
        {
            setFixed();
            stateMachine.triggers.setFixed = false;
        }

        if (stateMachine.triggers.setSolved)
        {
            setSolved();
            stateMachine.triggers.setSolved = false;
        }
	}

    void setSolved()
    {
        animator.SetTrigger("solved");
    }

    public void DelayedSwitchSprite ()
    {
        Invoke("SwitchSprite", 0.5f);
    }

    public void SwitchSprite ()
    {
        if (solvedSprite)
        {
            bufferSprite = spriteRend.sprite;
            spriteRend.sprite = solvedSprite;
            solvedSprite = bufferSprite;
        }
    }

    void setMoveable()
    {
        animator.SetBool("selected", false);
        animator.SetBool("floating", true);
        stateMachine.state = State.Moveable;
    }

    IEnumerator DelayedFloat()
    {
        animator.SetBool("selected", false);
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        animator.SetBool("floating", true);
    }

    void setFixed()
    {
        animator.SetBool("selected", false);
        animator.SetBool("floating", false);
        animator.SetBool("fixed", true);
        GetComponent<BoxCollider2D>().enabled = false;
        stateMachine.state = State.Fixed;
    }

    void OnMouseDown()
    {
        if (stateMachine.state != State.Selected)
        {
            animator.SetBool("selected", true);
            prevState = stateMachine.state;
            stateMachine.state = State.Selected;
            if (clickSounds.Length > 0)
                audioSource.PlayOneShot(clickSounds[Random.Range(0, clickSounds.Length)]);
        }
        else
        {
            animator.SetBool("selected", false);
            stateMachine.state = prevState;
        }

        if (onClickDelegate != null)
            onClickDelegate(transform.parent.gameObject);   // If parent transform is global position
            //onClickDelegate(gameObject);                  // If self transform is global position
    }
}
