using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldPlayer : MonoBehaviour
{
    WorldNPC npc;

    private void Awake()
    {
        npc = GetComponent<WorldNPC>();

    }
    private void Start()
    {
        npc.Mover.OnTileReached += MoveEnd;
    }
    // Update is called once per frame
    void Update()
    {
        npc.RunState = Keyboard.current.oKey.isPressed;
        Vector2 dpad = UserInput.GetDpad();

        npc.Direction = Vector2.zero;

        if (dpad.y > 0.5)
            npc.Direction = Vector2.up;
        if (dpad.y < -0.5)
            npc.Direction = Vector2.down;
        if (dpad.x < -0.5)
            npc.Direction = Vector2.left;
        if (dpad.x > 0.5)
            npc.Direction = Vector2.right;

        if (dpad.magnitude < 0.1f)
            return;


        float currentMoveTime = npc.MoveTime;

        if (npc.RunState)
            currentMoveTime *= 0.5f;

        npc.Mover.MoveInDirection(npc.Direction, currentMoveTime);
    }

    public void MoveEnd(Vector2 endPosition)
    {
        if (World.IsGrass(endPosition))
        {
            TryForEncounters();
        }
    }

    void TryForEncounters()
    {
        int chance = Random.Range(0, 256);
        if (chance < 200)
        {
            GameManager.EncouterPokemon();
        }

    }
}
