using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInput : MonoBehaviour
{
    public int controllerIndex = 0;

    private InputDevice device;

    public bool ControllerConnected { get { return device != null; } }

    //Controller mappings
    public InputControl moveX;
    public InputControl moveY;

    public InputControl cameraX;
    public InputControl cameraY;

    public InputControl jump;

    void Start()
    {
        device = InputManager.Devices[controllerIndex];

        if(device != null)
        {
            //Assign controller mappings
            moveX = device.GetControl(InputControlType.LeftStickX);
            moveY = device.GetControl(InputControlType.LeftStickY);

            cameraX = device.GetControl(InputControlType.RightStickX);
            cameraY = device.GetControl(InputControlType.RightStickY);

            jump = device.GetControl(InputControlType.Action1);
        }
    }
}
