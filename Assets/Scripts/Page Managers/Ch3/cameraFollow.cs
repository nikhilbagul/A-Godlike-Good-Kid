using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour
{
    [Header("Follow multipler")]
    public float multiplier = 1;

    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;
    public Vector3 minCameraPos, maxCameraPos;
    public GameObject iniPosition;
    // Use this for initialization


    void Start()
    {
        
        targetPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            //posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 5f * multiplier;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                                                 Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),-10);

        }
    }

   public void disableFollowing()
    {
        transform.GetComponent<cameraFollow>().enabled = false;
    }

    public void enableFollowing()
    {
        transform.GetComponent<cameraFollow>().enabled = true;
    }

    public void hasFinishedIntro()
    {
        GetComponent<Camera>().orthographicSize = 6;
        
    }
   
}
