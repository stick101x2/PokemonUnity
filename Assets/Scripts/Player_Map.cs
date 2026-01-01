using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Map : MonoBehaviour
{
    public LayerMask Grass;
    GridBasedMovement mover;
    float canMoveTimer = 0.25f;
    float resetMoveTimer = 0.1f;
    const float RESET_MOVE_DELAY = 0.1f;
    const float CAN_MOVE_DELAY = 0.1f;
    Vector2 dir;
    Animator anim;
    bool isMoving;
    bool atWall;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<GridBasedMovement>();
        anim = transform.GetChild(0).GetComponent<Animator>();

       // UserInput.instance.Dpad.started += MovementStart;
        UserInput.instance.Dpad.canceled += MovementEnd;

        mover.OnMoveStart += SetAnimDir;
        mover.OnTileReached += TryForEncounters;
        mover.OnTileReached += OnMoveEnd;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    private void LateUpdate()
    {
        Animation();
    }
    public void Movement()
    {
        Vector2 dpad = UserInput.GetDpad();

        if(!canMove || GameManager.inMenu)
            dpad = Vector2.zero;

        if (resetMoveTimer < RESET_MOVE_DELAY)
            resetMoveTimer += Time.deltaTime;
        atWall = false;
        if(dpad.magnitude < 0.2f) // 0f if no input is pressed otherwise 1f
        {
            canMoveTimer = CAN_MOVE_DELAY;
            return;
        }
        else if (canMoveTimer > 0f)
        {
            canMoveTimer -= Time.deltaTime;
            if(!mover.isMoving)
                SetAnimDir(dpad);
            if (canMoveTimer > 0f && resetMoveTimer >= RESET_MOVE_DELAY)
                return;

        }
        isMoving = true;
        bool didMove = false;
        if (dpad.y > 0.5)
            didMove = mover.MoveInDirection(Vector2.up);
        if (dpad.y < -0.5)
            didMove = mover.MoveInDirection(Vector2.down);
        if (dpad.x < -0.5)
            didMove = mover.MoveInDirection(Vector2.left);
        if (dpad.x > 0.5)
            didMove = mover.MoveInDirection(Vector2.right);

        if(!didMove)
        {
            atWall = true;
            isMoving = false;
        }
    }
    public void DisableMovement()
    {
        canMove = false;
        isMoving = false;
    }
    public void Animation()
    {
        anim.SetBool("isMoving", isMoving);
        if (atWall && GetDpadDirection().magnitude > 0.1f)
        {
            anim.SetBool("isMoving", true);
        }
    }

    void OnMoveEnd()
    {
        Vector2 dpad = UserInput.GetDpad();
        if (dpad.magnitude < 0.2f)
        {
            isMoving = false;
        }
    }
    public void MovementEnd(InputAction.CallbackContext context)
    {
        canMoveTimer = CAN_MOVE_DELAY;
        resetMoveTimer = 0f;
    }

    public void SetAnimDir(Vector2 movement)
    {
        dir = movement;
        anim.SetFloat("moveX", dir.x);
        anim.SetFloat("moveY", dir.y);
    }

    void TryForEncounters()
    {
       Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.25f, World.GetGrassLayer());
        if (hit != null)
        {
            int chance = Random.Range(0, 256);
            if (chance < 200)
            {
                DisableMovement();
                GameManager.EncouterPokemon();
                
            } 
        }
       
    }

    Vector2 GetDpadDirection()
    {
       Vector2 dpad = UserInput.GetDpad();

        if (dpad.y > 0.5)
            return Vector2.up;
        if (dpad.y < -0.5)
            return Vector2.down;
        if (dpad.x < -0.5)
            return Vector2.left;
        if (dpad.x > 0.5)
            return Vector2.right;

        return Vector2.zero;
    }

  
}
