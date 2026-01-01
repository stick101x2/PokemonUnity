using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldNPC : MonoBehaviour
{
    [SerializeField] SpriteRenderer spr;
    [SerializeField] Animator anim;

    [SerializeField] float moveTime = 0.25f;
    [SerializeField] Vector2 direction;

    bool isRunning;
    GridMover3D gridMover;

    public GridMover3D Mover { get { return gridMover; } }
    public bool RunState {  get { return isRunning; } set { isRunning = value; } }
    public float MoveTime {  get { return moveTime; }  }
    public Vector2 Direction {  get { return direction; } set { direction = value; } }

    public void Awake()
    {
        gridMover = GetComponent<GridMover3D>();

        gridMover.OnMoveStart += MoveStart;
        gridMover.OnTileReached += MoveEnd;
    }

    public void MoveStart(Vector2 direction)
    {
        anim.SetFloat("x_dir", direction.x);
        anim.SetFloat("y_dir", direction.y);

        if(isRunning)
            anim.Play("run");
        else
            anim.Play("walk");
    }
    public void MoveEnd(Vector2 endPosition)
    {
        if (direction.magnitude < 0.1f)
        {
            anim.Play("idle",0,0);
        }

    }

}
