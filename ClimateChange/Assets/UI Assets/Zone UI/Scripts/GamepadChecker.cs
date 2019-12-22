using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GH;

namespace Michsky.UI.Zone
{
    public class GamepadChecker : MonoBehaviour
    {
        [Header("RESOURCES")]
        public GameObject virtualCursor;
        public GameObject virtualCursorContent;
        public GameObject tooltipDesktop;
        public GameObject eventSystem;

        [Header("OBJECTS")]
        [Tooltip("Objects in this list will be active when gamepad is un-plugged.")]
        public List<GameObject> keyboardObjects = new List<GameObject>();
        [Tooltip("Objects in this list will be active when gamepad is plugged.")]
        public List<GameObject> gamepadObjects = new List<GameObject>();

        [Header("SETTINGS")]
        [Tooltip("Always update input device. If you turn off this feature, you won't able to change the input device after start, but it might increase the performance.")]
        public bool alwaysSearch = true;

        private TooltipManagerDesktop tooltipDesktopScript;
        private GamepadChecker checkerScript;
        protected string[] names;
        private int GamepadConnected = 0;
        private Vector3 startMousePos;
        private Vector3 startPos;

        protected bool gamepadEnabled;

        void Start()
        {
            tooltipDesktopScript = this.GetComponent<TooltipManagerDesktop>();
            checkerScript = gameObject.GetComponent<GamepadChecker>();

            SwitchToKeyboard();

            if (alwaysSearch == false)
            {
                checkerScript.enabled = false;
            }

            else
            {
                checkerScript.enabled = true;
                Debug.Log("Always Search is on. Input device will be updated in case of disconnecting/connecting.");
            }
        }

        void Update()
        {
            names = Input.GetJoystickNames();

            for (int x = 0; x < names.Length; x++)
            {
                print(names[x].Length); //Just for testing stuff

                if (names[x].Length >= 1)
                {
                    GamepadConnected = 1;
                }

                else if (names[x].Length == 0)
                {
                    GamepadConnected = 0;
                }
            }

            if (GamepadConnected == 1 && gamepadEnabled == false)
            {
                SwitchToController();
            }

            else if (GamepadConnected == 0 && gamepadEnabled == true)
            {
                SwitchToKeyboard();
            }
        }

        public void CheckControllerType(int i)
        {
            if (i == 19)
            {
                Debug.Log("This should be setup for a PS4 Controller");
                GH.EventSystem.instance.RaiseEvent(new ChangeInputType
                {
                    platform = Platform.PS4
                });
                //switch the input to be for PS4 as well as the UI prompts
            }
            else if(i == 20)
            {
                Debug.Log("This should be setup for a Logitech Controller");
                GH.EventSystem.instance.RaiseEvent(new ChangeInputType
                {
                    platform = Platform.Logitech
                });
            }
            else if (i == 33)
            {
                Debug.Log("This should be setup for an Xbox Controller");
                GH.EventSystem.instance.RaiseEvent(new ChangeInputType
                {
                    platform = Platform.Xbox
                });
                //swutch the input to be for Xbox Controller as well as the UI prompts
            }
            // ... check for logitech and any other controller types we need
            // these are arbitrary numbers Unity made to identify controller types in GetJoySticksNames function
        }

        public virtual void SwitchToController()
        {
            for (int i = 0; i < keyboardObjects.Count; i++)
            {
                keyboardObjects[i].SetActive(false);
            }

            for (int i = 0; i < gamepadObjects.Count; i++)
            {
                gamepadObjects[i].SetActive(true);
            }

            gamepadEnabled = true;
            eventSystem.SetActive(false);

            //should switch next
            CheckControllerType(names[0].Length);


            virtualCursor.SetActive(true);

            if(names[0].Length == 20)
            {
                virtualCursor.GetComponent<VirtualCursor>().horizontalAxis = "Logitech_RightJoyStickX";
                virtualCursor.GetComponent<VirtualCursor>().verticalAxis = "Logitech_RightJoyStickY";
            }
            else if(names[0].Length == 33)
            {
                virtualCursor.GetComponent<VirtualCursor>().horizontalAxis = "XboxOne_RightJoyStickX";
                virtualCursor.GetComponent<VirtualCursor>().verticalAxis = "XboxOne_RightJoyStickY";
            }
            else if((names[0].Length == 19))
            {
                virtualCursor.GetComponent<VirtualCursor>().horizontalAxis = "PS4_RightJoyStickX";
                virtualCursor.GetComponent<VirtualCursor>().verticalAxis = "PS4_RightJoyStickY";
            }
           
            virtualCursorContent.SetActive(true);
            tooltipDesktop.SetActive(false);
            tooltipDesktopScript.enabled = false;
            Debug.Log("Gamepad detected. Switching to gamepad input.");
        }

        public virtual void SwitchToKeyboard()
        {
            for (int i = 0; i < keyboardObjects.Count; i++)
            {
                keyboardObjects[i].SetActive(true);
            }

            for (int i = 0; i < gamepadObjects.Count; i++)
            {
                gamepadObjects[i].SetActive(false);
            }

            gamepadEnabled = false;
            virtualCursor.SetActive(false);
            virtualCursorContent.SetActive(false);
            tooltipDesktop.SetActive(true);
            tooltipDesktopScript.enabled = true;
            eventSystem.SetActive(true);
            Debug.Log("No gamepad detected. Switching to keyboard input.");
        }
    }
}