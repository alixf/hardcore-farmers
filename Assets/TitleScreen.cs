using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    public RectTransform hardcore;
    public RectTransform farmers;
    public RectTransform trad;
    public RectTransform play;
    public Animator farmer1;
    public Animator farmer2;

    void Start ()
    {
        StartCoroutine(Coroutines.Move2D(hardcore, hardcore.anchoredPosition + new Vector2(0, 100), hardcore.anchoredPosition, 0.33f, Easings.ExpoEaseIn, 2f));
        StartCoroutine(Coroutines.Fade(hardcore.GetComponent<CanvasGroup>(), 0f, 1f, 0.33f, Easings.Linear, 2f));

        StartCoroutine(Coroutines.Move2D(farmers, hardcore.anchoredPosition + new Vector2(0, 100), farmers.anchoredPosition, 0.33f, Easings.ExpoEaseIn, 3f));
        StartCoroutine(Coroutines.Fade(farmers.GetComponent<CanvasGroup>(), 0f, 1f, 0.33f, Easings.Linear, 3f));
        StartCoroutine(Coroutines.Fade(trad.GetComponent<CanvasGroup>(), 0f, 1f, 0.33f, Easings.Linear, 3f));

        StartCoroutine(Coroutines.Move2D(play, hardcore.anchoredPosition + new Vector2(0, 100), play.anchoredPosition, 0.33f, Easings.ExpoEaseIn, 4f));
        StartCoroutine(Coroutines.Fade(play.GetComponent<CanvasGroup>(), 0f, 1f, 0.33f, Easings.Linear, 4f));

        StartCoroutine(Coroutines.SetTrigger(farmer1, "stomp", 1.75f));
        StartCoroutine(Coroutines.SetTrigger(farmer2, "stomp", 1.75f));
        StartCoroutine(Coroutines.SetTrigger(farmer1, "stomp", 2.75f));
        StartCoroutine(Coroutines.SetTrigger(farmer2, "stomp", 2.75f));
        StartCoroutine(Coroutines.SetTrigger(farmer1, "stomp", 3.75f));
        StartCoroutine(Coroutines.SetTrigger(farmer2, "stomp", 3.75f));
    }
	
	void StartGame() {
	    
	}
}
