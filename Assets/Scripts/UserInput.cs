using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class UserInput : MonoBehaviour
{
    PlayerInput playerInput;
    public PlayerInput PlayerInput { get { return playerInput; } }
    public static UserInput instance;
    public InputAction Dpad { get; private set; }
    

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        playerInput = GetComponent<PlayerInput>();


        Dpad = instance.playerInput.actions["Move"];
        
    }

    public static Vector2 GetDpad()
    {
        return instance.playerInput.actions["Move"].ReadValue<Vector2>();
        
    }
}
