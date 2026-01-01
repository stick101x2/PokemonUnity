using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveToPoint : MonoBehaviour
{
    public AnimationCurve curve;

    [SerializeField] float duration = 1.0f;
    [SerializeField] float heightY = 1.0f;
    Vector2 beginning;
    Vector2 ending;
    [SerializeField] Transform endTarget;
    [SerializeField] Transform startPosition;
    // Start is called before the first frame update
    void Start()
    {
        beginning = startPosition.position;
        ending = endTarget.position;

        StartCoroutine(Curve(beginning, ending));
    }
    public IEnumerator Curve(Vector2 start, Vector2 target)
    {
        float timePassed = 0f;

        Vector2 end = target;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;

            float linearTime = timePassed / duration;
            float heightTime = curve.Evaluate(linearTime);

            float height = Mathf.Lerp(0f, heightY, heightTime);

            transform.position = Vector2.Lerp(start, end, linearTime) + new Vector2(0f, height);
        
            yield return null;
        }
    }
}
