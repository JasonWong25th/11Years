using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Zone;
using GH;

public class ChangeInputType : GH.Event
{
    public Platform platform;
}

public class ControllerDetection : GamepadChecker
{
    public override void SwitchToKeyboard()
    {
        gamepadEnabled = false;
        EventSystem.instance.RaiseEvent(new ChangeInputType {
            platform = Platform.PC
        });
    }
    public override void SwitchToController()
    {
        gamepadEnabled = true;
        CheckControllerType(names[0].Length);
    }
}
