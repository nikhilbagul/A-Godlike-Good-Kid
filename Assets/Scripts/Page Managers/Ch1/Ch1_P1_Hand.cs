using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ch1_P1_Hand : MonoBehaviour
{
    public enum Hand { Left, Right };
    public enum State { Free, Anchored };

    [SerializeField]
    private Hand hand;
    public Hand _hand { get { return hand; } }

    [SerializeField]
    private Ch1_P1_MapHandler handler;

    private State state;
    [Header("Status")]
    public State _state;

    private Vector3 initPos, offset, mousePos;

	void Start ()
    {
        initPos = transform.position;
        GetRefs();
	}

    void GetRefs()
    {
        if (!handler)
            handler = transform.parent.GetComponent<Ch1_P1_MapHandler>();
        if (!handler)
            Debug.Log("Set Map Hanlder reference or keep both Hands as its children.");
    }

    void Update ()
    {
        _state = state;
        

        if (state == State.Anchored)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = (mousePos - offset); // * handler.MovementMultiplier;
        }

        if (state == State.Free && transform.position != initPos)
        {
            transform.position = Vector3.Lerp(transform.position, initPos, 0.5f * Time.deltaTime * 10);
            if ((transform.position - initPos).sqrMagnitude > 0.1f)
                transform.position = initPos;
        }
    }

    void OnMouseDown()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (state != State.Anchored)
            offset = mousePos - transform.position;

        state = State.Anchored;
    }

    void OnMouseUp()
    {
        state = State.Free;
    }
}
