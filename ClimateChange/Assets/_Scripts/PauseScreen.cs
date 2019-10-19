using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class PauseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        EventSystem.instance.RaiseEvent(new Restart { });
    }

    public void Resume()
    {
        GameManager.Instance.pause = false;
        EventSystem.instance.RaiseEvent(new PauseGame { pause = false });
    }
}
