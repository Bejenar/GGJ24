using System.Collections;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    private AnimationCurve toRight = AnimationCurve.EaseInOut(0, 0, 1, 19.2f);
    private AnimationCurve toLeft = AnimationCurve.EaseInOut(0, 19.2f, 1, 0);

    public bool react = true;

    public void MoveToRight()
    {
        if (react)
        {
            StartCoroutine(PlaySpriteAnimation(toRight));
        }
    }

    public void MoveToLeft()
    {
        if (react)
        {
            StartCoroutine(PlaySpriteAnimation(toLeft));
        }
    }

    private IEnumerator PlaySpriteAnimation(AnimationCurve curve)
    {
        var timeElapsed = 0f;
        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime;

            var currentValue = curve.Evaluate(timeElapsed);
            this.transform.position = new Vector3(currentValue, 0, -10);
            yield return null;
        }
    }
}