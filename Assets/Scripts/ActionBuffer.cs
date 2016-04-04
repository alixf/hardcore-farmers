using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActionBuffer : MonoBehaviour {

    public RectTransform sKeyPrefab;
    public RectTransform dKeyPrefab;
    public RectTransform jKeyPrefab;
    public RectTransform kKeyPrefab;
    public RectTransform hKeyPrefab;
    public RectTransform leftKeyPrefab;
    public RectTransform rightKeyPrefab;
    public RectTransform downKeyPrefab;
    public RectTransform upKeyPrefab;

    RectTransform rect;
    public int index = 0;
    private Vector2 initialPosition;

    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        initialPosition = rect.anchoredPosition;
    }

    IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public void SetActionBuffer(List<TimedKey> keys)
    {
        if(keys == null)
        {
            StartCoroutine(Coroutines.Move2D(rect, rect.anchoredPosition + new Vector2(0, -100), 0.15f));
            StartCoroutine(Coroutines.Fade(GetComponent<CanvasGroup>(), 0f, 0.25f));
            StartCoroutine(ClearAfterDelay(0.25f));
        }
        else
        {
            int count = 0;
            foreach (var key in keys)
            {
                RectTransform keyUI = null;
                switch (key.key)
                {
                    case KeyCode.S: keyUI = Instantiate(sKeyPrefab) as RectTransform; break;
                    case KeyCode.D: keyUI = Instantiate(dKeyPrefab) as RectTransform; break;
                    case KeyCode.J: keyUI = Instantiate(jKeyPrefab) as RectTransform; break;
                    case KeyCode.K: keyUI = Instantiate(kKeyPrefab) as RectTransform; break;
                    case KeyCode.LeftArrow: keyUI = Instantiate(leftKeyPrefab) as RectTransform; break;
                    case KeyCode.RightArrow: keyUI = Instantiate(rightKeyPrefab) as RectTransform; break;
                    case KeyCode.DownArrow: keyUI = Instantiate(downKeyPrefab) as RectTransform; break;
                    case KeyCode.UpArrow: keyUI = Instantiate(upKeyPrefab) as RectTransform; break;
                }
                keyUI.SetParent(transform);
                keyUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(100 * count, -200);
                if (count > 0)
                    keyUI.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                else
                    keyUI.localScale = new Vector3(1f, 1f, 1f);
                ++count;
            }
            index = 0;
            
            StartCoroutine(Coroutines.Move2D(rect, initialPosition + new Vector2(0, -100), initialPosition, 0.15f));
            StartCoroutine(Coroutines.Fade(GetComponent<CanvasGroup>(), 1f, 0.25f));
        }
    }

    public void forward()
    {
        StartCoroutine(Coroutines.Scale(transform.GetChild(index), new Vector3(0.8f, 0.8f, 0.8f), 0.15f));
        index++;
        if (index < transform.childCount)
            StartCoroutine(Coroutines.Scale(transform.GetChild(index), new Vector3(1f, 1f, 1f), 0.15f));
        RectTransform rt = GetComponent<RectTransform>();
        StartCoroutine(Coroutines.Move2D(rt, new Vector2(index * -100, rt.anchoredPosition.y), 0.15f));

    }

    public void backward()
    {
        if (index > 0)
        {
            StartCoroutine(Coroutines.Scale(transform.GetChild(index), new Vector3(0.8f, 0.8f, 0.8f), 0.15f));
            index--;
            StartCoroutine(Coroutines.Scale(transform.GetChild(index), new Vector3(1f, 1f, 1f), 0.15f));
            RectTransform rt = GetComponent<RectTransform>();
            StartCoroutine(Coroutines.Move2D(rt, new Vector2(index * -100, rt.anchoredPosition.y), 0.15f));
        }
    }

    public void SetFill(float fill)
    {
        Image fillImage = transform.GetChild(index).GetChild(0).GetComponent<Image>();
        fillImage.fillAmount = fill;
    }
    
    public void SetRepeatKey(KeyCode key)
    {
        RectTransform keyUI = null;
        switch (key)
        {
            case KeyCode.S: keyUI = Instantiate(sKeyPrefab) as RectTransform; break;
            case KeyCode.D: keyUI = Instantiate(dKeyPrefab) as RectTransform; break;
            case KeyCode.J: keyUI = Instantiate(jKeyPrefab) as RectTransform; break;
            case KeyCode.K: keyUI = Instantiate(kKeyPrefab) as RectTransform; break;
            case KeyCode.H: keyUI = Instantiate(hKeyPrefab) as RectTransform; break;
            case KeyCode.LeftArrow: keyUI = Instantiate(leftKeyPrefab) as RectTransform; break;
            case KeyCode.RightArrow: keyUI = Instantiate(rightKeyPrefab) as RectTransform; break;
            case KeyCode.DownArrow: keyUI = Instantiate(downKeyPrefab) as RectTransform; break;
            case KeyCode.UpArrow: keyUI = Instantiate(upKeyPrefab) as RectTransform; break;
        }
        keyUI.SetParent(transform);
        keyUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -250);
        keyUI.localScale = new Vector3(1f, 1f, 1f);
        keyUI.gameObject.AddComponent<Shake>();

        StartCoroutine(Coroutines.Move2D(rect, initialPosition + new Vector2(0, -100), initialPosition, 0.15f));
        StartCoroutine(Coroutines.Fade(GetComponent<CanvasGroup>(), 1f, 0.25f));
    }

    public void RemoveRepeatKey()
    {
        StartCoroutine(Coroutines.Move2D(rect, rect.anchoredPosition + new Vector2(0, -100), 0.15f));
        StartCoroutine(Coroutines.Fade(GetComponent<CanvasGroup>(), 0f, 0.25f));
        StartCoroutine(ClearAfterDelay(0.25f));
    }

    public void Cleanup()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }
}
