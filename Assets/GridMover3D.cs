using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.InputSystem;

public class GridMover3D : MonoBehaviour
{
    public bool isMoving { get; private set; }
    public Vector3 currrentMovement { get; private set; }

    Vector3 originalPosition;
    Vector3 targetPosition;

    public event Action<Vector2> OnMoveStart;
    public event Action<Vector2> OnTileReached;
    public bool MoveInDirection(Vector2 direction, float moveTime)
    {
        if (isMoving)
            return true;
        /*if (!isWalkable(direction))
            return false;*/
        OnMoveStart?.Invoke(direction);
        StartCoroutine(MoveEntity(direction, moveTime));
        return true;
    }
    IEnumerator MoveEntity(Vector2 direction, float currentMoveTime)
    {
        isMoving = true;

        float elapsedTime = 0;

        originalPosition = transform.position;
        targetPosition = new Vector3(originalPosition.x + direction.x, originalPosition.y + direction.y,originalPosition.z);
        
        float height = World.GetTileHeight(targetPosition);

        if (!float.IsNaN(height)) 
            targetPosition.z = height;
            
        
        while (elapsedTime < currentMoveTime)
        {
            currrentMovement = new Vector3(direction.x, direction.y, 0);

            transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / currentMoveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        OnTileReached?.Invoke(targetPosition);

        currrentMovement = Vector3.zero;
        transform.position = targetPosition;
        isMoving = false;
       // step++;
    }

    
    /*
    bool isWalkable(Vector2 direction)
    {
        Vector3 targetPos = transform.position + (Vector3)direction;
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.25f, CollisonLayer);
        if (hit != null)
        {
            return false;
        }
        return true;
    }*/
}
