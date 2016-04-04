using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    public void Shake()
    {
        StartCoroutine(ShakeCoroutine(0.1f, 0.04f, 20f));
    }

    public IEnumerator ShakeCoroutine(float duration, float amplitude, float frequency)
    {
        Vector3 pos = transform.position;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            transform.position = pos + Vector3.up * Mathf.Sin(clock * frequency * 2f * Mathf.PI) * amplitude;
            yield return null;
        }
        transform.position = pos;
    }
}
