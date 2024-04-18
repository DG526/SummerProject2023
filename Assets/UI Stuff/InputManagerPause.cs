using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerPause : MonoBehaviour
{
    public static InputManagerPause instance;

    public bool MenuInput {get; private set;}

    private PlayerInput pInput;

    private InputAction pAction;

    InputAction IAPause;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //pInput = GetComponent<PlayerInput>();
        //pAction = pInput.actions["MenuOpenClose"];

        IAPause = new InputAction();
        IAPause.AddBinding("<Keyboard>/escape");
        IAPause.AddBinding("<Gamepad>/start");
        IAPause.Enable();
    }

    private void Update()
    {
        //MenuInput = pAction.WasPerformedThisFrame();
        MenuInput = IAPause.triggered;
    }
}
