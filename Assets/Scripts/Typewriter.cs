using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Typewriter : MonoBehaviour
{
    [Header("Input Parameters")]
    public Text target;
    public UnityEngine.Audio.AudioMixerSnapshot mute;
    [Tooltip("Leave empty if not used")]
    public Text footer;

    [System.Serializable]
    public struct TextAndFont
    {
        [TextArea(4,15)]
        public string text;
        [Tooltip("Leave empty if no change")]
        public Font font;
    }

    [SerializeField]
    public TextAndFont[] textToDisplay;

    //[TextArea]
    //public string[] textToDisplay;
    public bool autoStart = true;
    public float autoStartDelay = 1f;

    [System.Serializable]
    public enum TypeState { idle, typing, completed };
    private TypeState currentState;
    public TypeState state { get { return currentState; } }

    [Header("Status")] [Tooltip("Only displays status, changing it manually will not have any effect.")]
    public TypeState stateCopy;

	IEnumerator flushtext;

	public int currentPage;
	public GameObject writingTable;
	public GameObject textdisplayArea;

    void Start()
    {
        if (autoStart)
            StartTyping();

        currentState = TypeState.idle;
		currentPage = 0; 

    }

    void Update()
    {
        stateCopy = currentState;
    }

    public void StartTyping()
    {
        if (textToDisplay[currentPage].font)
            target.font = textToDisplay[currentPage].font;

        flushtext = FlushText ();

		StartCoroutine(flushtext);

        if (footer)
            footer.text = (currentPage + 1) + " / " + textToDisplay.Length;
    }

	public void StopTyping(){
		if (target.text.Length > 10 &&currentState == TypeState.typing) {
			StopCoroutine (flushtext);
			target.text = textToDisplay[currentPage].text;
			currentState = TypeState.completed;
			currentPage += 1;
		} 
		else if(currentState==TypeState.completed && currentPage < textToDisplay.Length){
			//currentPage += 1;
			StartTyping ();
		}
		else if(currentState==TypeState.completed && currentPage >= textToDisplay.Length){
			currentPage = 0;
			writingTable.SetActive (true);
			textdisplayArea.SetActive (false);

            FindObjectOfType<MusicManager>().TransitionTo(mute);
		}

	}
	//when tap on scren, stop coroutine, display all the text

    public IEnumerator FlushText()
    {
        if (autoStart)
            yield return new WaitForSeconds(autoStartDelay);

        currentState = TypeState.typing;
        
        target.text = "";
		int length = textToDisplay[currentPage].text.Length;

        for (int index=0; index < length; ++index)
        {
			target.text += textToDisplay[currentPage].text[index];
            yield return null;
        }
		currentPage += 1;
        currentState = TypeState.completed;
    }


}
