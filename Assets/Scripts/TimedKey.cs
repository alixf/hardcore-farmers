using UnityEngine;
using System.Collections;

public class TimedKey {
    public KeyCode key;
    public float time;

    public TimedKey(KeyCode key, float time)
    {
        this.key = key;
        this.time = time;
    }

    public static implicit operator TimedKey(KeyCode key)
    {
        return new TimedKey(key, 0f);
    }
}
