using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {
    public float amplitude = 0.15f;
    public float clock = 0f;
    public float frequency = 5f;

	void Update () {
        clock += Time.deltaTime;
        transform.localScale = Vector3.one + Vector3.one * (Mathf.Sin(clock * frequency * 2f * Mathf.PI) + 1f) / 2f * amplitude;
    }
}
