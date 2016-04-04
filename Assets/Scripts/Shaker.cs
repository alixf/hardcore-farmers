using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour {
    public void ShakeCamera()
    {
        Camera.main.transform.parent.GetComponent<CameraShake>().Shake();
    }
}
