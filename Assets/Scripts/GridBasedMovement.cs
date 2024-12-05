using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridBasedMovement : MonoBehaviour
{
    public float timeToMove = 0.2f;
    public Vector2Int move;
    public bool hasCollison;
    public LayerMask CollisonLayer;

    public bool isMoving { get; private set; }
    Vector2 originalPosition;
    Vector2 targetPosition;
    public event Action OnTileReached;
    public event Action<Vector2> OnMoveStart;
    public int step;
    void Update()
    {
        MoveInDirection(move);
    }
    IEnumerator MoveEntity(Vector2 direction)
    {
        isMoving = true;
        
        float elapsedTime = 0;

        originalPosition = transform.position;
        targetPosition = originalPosition + direction;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector2.Lerp(originalPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OnTileReached?.Invoke();
        
        transform.position = targetPosition;
        isMoving = false;
        step++;
    }

    public bool MoveInDirection(Vector2 direction)
    {
        if (isMoving)
            return true;
        if (!isWalkable(direction))
            return false;
        OnMoveStart?.Invoke(direction);  
        StartCoroutine(MoveEntity(direction));
        return true;
    }

    bool isWalkable(Vector2 direction)
    {
        Vector3 targetPos = transform.position + (Vector3)direction;
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.25f, CollisonLayer);
        if (hit != null)
        {
            return false;
        }
        return true;
    }
}
