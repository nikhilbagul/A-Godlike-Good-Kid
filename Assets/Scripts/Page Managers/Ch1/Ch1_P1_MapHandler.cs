using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ch1_P1_MapHandler : MonoBehaviour
{
    public Ch1_P1_Hand leftHand, rightHand;

    [SerializeField]
    private Vector2 triggerStart, triggerEnd;

    [SerializeField][Header("Status only")]
    private Vector2 currentDist;

    private Ch1_P2 pageManager;
    private Transform left, right;

    void Start()
    {
        GetHandRefs();
    }

    void GetHandRefs()
    {
        if (!leftHand || !rightHand)
        {
            Ch1_P1_Hand[] hands = GetComponentsInChildren<Ch1_P1_Hand>();
            foreach (Ch1_P1_Hand hand in hands)
            {
                if (hand._hand == Ch1_P1_Hand.Hand.Left)
                    leftHand = hand;
                else if (hand._hand == Ch1_P1_Hand.Hand.Right)
                    rightHand = hand;
            }
        }

        if (!leftHand || !rightHand)
            Debug.Log("Set Left and Right hand references or keep them as children.");

        pageManager = FindObjectOfType<Ch1_P2>();

        left = leftHand.GetComponent<Transform>();
        right = rightHand.GetComponent<Transform>();
    }

    void Update ()
    {
        CheckHandDistance();
    }

    void CheckHandDistance()
    {
        bool x = false, y = false;

        float x_dist = right.position.x - left.position.x;
        float y_dist = Mathf.Abs(left.position.y - right.position.y);

        // To show in editor
        currentDist = new Vector2(x_dist, y_dist);

        if (x_dist <= triggerStart.x && x_dist >= triggerEnd.x)
            x = true;
        else x = false;

        if (y_dist <= triggerStart.y && y_dist >= triggerEnd.y)
            y = true;
        else y = false;

        if (x && y)
        {
            // Fix positions and disable colliders
            Destroy(leftHand);
            Destroy(rightHand);
            pageManager.PuzzleSolved();
            Destroy(gameObject);
        }
    }
}
