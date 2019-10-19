using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.Zone
{
    public class PressKey : MonoBehaviour
    {
        [Header("KEY")]
        [SerializeField]
        public KeyCode hotkey;
        public bool pressAnyKey;

        [Header("KEY ACTION")]
        [SerializeField]
        public UnityEvent pressAction;

        void Update()
        {
            if(pressAnyKey == true)
            {
                if (Input.anyKeyDown)
                {
                    pressAction.Invoke();
                } 
            }

            else
            {
                if (Input.GetKeyDown(hotkey))
                {
                    pressAction.Invoke();
                } 
            }
        }
    }
}