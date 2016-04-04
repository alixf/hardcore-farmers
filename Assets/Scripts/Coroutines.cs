using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coroutines : MonoBehaviour
{

    public static IEnumerator Move(Transform target, Vector3 position, float duration, System.Func<float, float, float, float, float> easing = null)
    {
        if (easing == null)
            easing = Easings.Linear;

        Vector3 start = target.position;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.position = Vector3.Lerp(start, position, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.position = position;
    }
    public static IEnumerator Move(Transform target, Vector3 from, Vector3 to, float duration, System.Func<float, float, float, float, float> easing = null)
    {
        if (easing == null)
            easing = Easings.Linear;

        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.position = Vector3.Lerp(from, to, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.position = to;
    }
    public static IEnumerator MoveLocal(Transform target, Vector3 localPosition, float duration, System.Func<float, float, float, float, float> easing = null)
    {
        if (easing == null)
            easing = Easings.Linear;

        Vector3 start = target.localPosition;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.localPosition = Vector3.Lerp(start, localPosition, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.localPosition = localPosition;
    }
    public static IEnumerator Move2D(RectTransform target, Vector2 position, float duration, System.Func<float, float, float, float, float> easing = null)
    {
        if (easing == null)
            easing = Easings.Linear;

        Vector3 start = target.anchoredPosition;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.anchoredPosition = Vector3.Lerp(start, position, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.anchoredPosition = position;
    }
    public static IEnumerator Move2D(RectTransform target, Vector2 from, Vector2 to, float duration, System.Func<float, float, float, float, float> easing = null, float delay = 0f)
    {
        target.anchoredPosition = from;
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (easing == null)
            easing = Easings.Linear;

        target.anchoredPosition = from;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.anchoredPosition = Vector3.Lerp(from, to, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.anchoredPosition = to;
    }
    public static IEnumerator Scale(Transform target, Vector3 scale, float duration, System.Func<float, float, float, float, float> easing = null)
    {
        if (easing == null)
            easing = Easings.Linear;

        Vector3 start = target.localScale;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.localScale = Vector3.Lerp(start, scale, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.localScale = scale;
    }
    public static IEnumerator Fade(CanvasGroup target, float alpha, float duration, System.Func<float, float, float, float, float> easing = null)
    {
        if (easing == null)
            easing = Easings.Linear;

        float start = target.alpha;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.alpha = Mathf.Lerp(start, alpha, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.alpha = alpha;
    }
    public static IEnumerator Fade(CanvasGroup target, float from, float to, float duration, System.Func<float, float, float, float, float> easing = null, float delay = 0f)
    {
        target.alpha = from;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (easing == null)
            easing = Easings.Linear;
        
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.alpha = Mathf.Lerp(from, to, easing(clock / duration, 0f, 1f, 1f));
            yield return null;
        }
        target.alpha = to;
    }
    public static IEnumerator Fade(MeshRenderer target, float from, float to, float duration, System.Func<float, float, float, float, float> easing = null)
    {
        if (easing == null)
            easing = Easings.Linear;

        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            target.material.color = new Color(target.material.color.r, target.material.color.g, target.material.color.b, Mathf.Lerp(from, to, easing(clock / duration, 0f, 1f, 1f)));
            yield return null;
        }
        target.material.color = new Color(target.material.color.r, target.material.color.g, target.material.color.b, to);
    }
    public static IEnumerator Disable(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(false);
    }
    public static IEnumerator Enable(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(true);
    }
    public static IEnumerator Destroy(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }
    public static IEnumerator Play(AudioSource target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.Play();
    }
    public static IEnumerator SetTrigger(Animator target, string trigger, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.SetTrigger(trigger);
    }
}
