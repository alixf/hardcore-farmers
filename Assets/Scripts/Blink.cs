using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

    public float minAlpha = 0.5f;
    public float maxAlpha = 1f;
    public float frequency = 1f;

    float clock = 0f;
    SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

	void Update () {
        clock += Time.deltaTime;
        float factor = Mathf.Sin(clock * frequency * Mathf.PI * 2f) / 2f + 0.5f;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(minAlpha, maxAlpha, factor));
	}
}
