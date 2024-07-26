using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Fungus;

public class TapToMove : MonoBehaviour
{
    public GameObject[] points;
    public AudioClip[] footsteps;

    [SerializeField]
    private Vector3 targetPosition; 
    private int counter = 0;
    float duration = 2f;
    float t = 0;
    float mouseX, mouseY;
    Vector2 mousePosition;
    bool shouldMove;
    static int count = 0;
    public static int index =0;
    public Flowchart flowchart;
    bool hasCalled = false;
    public bool FailedWithoutDrag = false;

    void Start()
    {        
        //Debug.Log(manager.hasChangedLayer);
        //Debug.Log(index);
        
        //targetPosition = points[5].transform.localPosition;
        if(count >= 2)
        {
            flowchart.SendFungusMessage("display hint one");
        }

        if(count == 0)
        {
            flowchart.SendFungusMessage("begin intro");
        }
        else if(count >= 1)
        {
            flowchart.SendFungusMessage("has reload");
        }

    }

   void Update()
    {
       if(shouldMove && counter>= 0 && counter < points.Length -1)
        {
            clickToMove();
        }
        if (counter == 29 && !hasCalled)
        {            
            Invoke("FadeAndReload", 1);
            hasCalled = true;
        }

    }

    void clickToMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 0));
            flowchart.SendFungusMessage("fade out sbs");

            GameObject f = GameObject.Find("Footsteps SFX");
            if (f)
            {
                AudioSource a = f.GetComponent<AudioSource>();
                a.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
            }
                

            if (mousePosition.y < transform.position.y)
            {
                t += Time.deltaTime;
                ++counter;
                //Debug.Log(counter);
                //Debug.Log("self:" + transform.position);            
                //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, points[counter].transform.position, Mathf.SmoothStep(0f,1f,t));
                Fungus.iTween.MoveTo(gameObject, points[counter].transform.position, 1.5f);
                playAnimation();
                if(counter > 28)
                {
                    flowchart.SendFungusMessage("time to die");
                }
            }
            else if (mousePosition.y > transform.position.y && counter > 0)
            {
                counter--;
                //Debug.Log(counter);
                //Debug.Log("self:" + transform.position);
                Fungus.iTween.MoveTo(gameObject, points[counter].transform.position, 1.5f);
                playAnimation();
            }
        }
    }

   public void setInactive()
    {
        shouldMove = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }



   public void setActive()
    {
        shouldMove = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void FadeAndReload()
    {
        if(GameObject.Find("Ch3_P2").GetComponent<Ch3_P2>().hasChangedLayer)
        {
            index++;
        }
        ++count;
        Fungus.iTween.CameraFadeAdd();
        Fungus.iTween.CameraFadeTo(1.0f, 1.0f);
        StartCoroutine(wait());
        //flowchart1.SendFungusMessage("wait");
      
    }

    void playAnimation ()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            Debug.Log("playing!");
            GetComponent<Animation>().Play("Faye and Ernest walking");
        }
    }


    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

  
   
}
