using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//To see which platform we're on
public enum Platform
{
    PC,
    Xbox,
    Mobile,
    PS4

}
public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance //Ensures that this is the only instance in the class
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    public readonly Platform Platform = Platform.PC;
}
