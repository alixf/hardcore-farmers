using UnityEngine;
using System.Collections;

public class MovingCamera : MonoBehaviour {

    public float duration;
    public Coroutine movingCoroutine;

	public void MoveTo(Vector3 position) {
        if (movingCoroutine != null)
            StopCoroutine(movingCoroutine);
        movingCoroutine = StartCoroutine(Coroutines.Move(transform, position, duration));
    }
}
