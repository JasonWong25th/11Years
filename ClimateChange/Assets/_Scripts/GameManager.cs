using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

//To see which platform we're on
public enum Platform
{
    PC,
    Xbox,
    Mobile,
    Logitech

}

public class AcidityBleachAudio : GH.Event
{
    public string type;
}

public class UpdatePoints : GH.Event
{
    public int deltaPoints;
}

public class UpdateOceanAcidificationUI :GH.Event
{
    public float oceanAcidicfaction;
}

public class PauseGame : GH.Event
{
    public bool pause;
}


public class GameManager : MonoBehaviour {
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance //Ensures that this is the only instance in the class
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>(); ;
            }
            return _instance;
        }
    }
    #endregion

    public Platform Platform = Platform.PC;
    public bool pause = false;
    public Player player;

    [SerializeField]
    [Range(0,40)]
    private float m_OceanAcidfication;
    //private float m_currentTime;
    //private float m_WaitTimeTillDeplete = 10f;
    [SerializeField]
    private float healthDepletionCoEff = 1;
    [SerializeField]
    private float oceanExpanisionCoEff = 1;

    private bool alive = true;
    public int points = 0;

    public bool finished2Objectives = false;

    private bool playedAcidAudio = false;
    private bool playedBleachAudio = false;

    private void Awake()
    {
        EventSystem.instance.AddListener<OnFinishEvent>(ChangeOceanAcidfcation);
        EventSystem.instance.AddListener<PlayerDie>(OnPlayerDied);
        EventSystem.instance.AddListener<ChangeInputType>(UpdateCurrentInput);
        EventSystem.instance.AddListener<PauseGame>(PauseGame);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<OnFinishEvent>(ChangeOceanAcidfcation);
        EventSystem.instance.RemoveListener<PlayerDie>(OnPlayerDied);
        EventSystem.instance.RemoveListener<ChangeInputType>(UpdateCurrentInput);
        EventSystem.instance.RemoveListener<PauseGame>(PauseGame);

    }

    private void Update()
    {
        //m_currentTime += Time.deltaTime;
        //if (m_currentTime >= m_WaitTimeTillDeplete)
        //{
        //    m_currentTime = 0.0f;
        //}
        //else
        //{
        //}
//        player.isInControl = !pause;

        #region OceanAcidicfcation
        if (finished2Objectives && !pause)
            {
                m_OceanAcidfication += Time.deltaTime * oceanExpanisionCoEff;
            }


            if (m_OceanAcidfication > 15)
            {
                healthDepletionCoEff = 1.25f;
                if (!playedBleachAudio)
                {
                    EventSystem.instance.RaiseEvent(new AcidityBleachAudio
                    {
                        type = "bleach"
                    });
                    playedBleachAudio = true;
                }

            }
            if (m_OceanAcidfication > 20)
            {
                healthDepletionCoEff += 1.5f;
            }
            if (m_OceanAcidfication > 25)
            {
                healthDepletionCoEff += 2f;
            }
            if (m_OceanAcidfication > 30)
            {
                healthDepletionCoEff += 3f;
            }

            if (m_OceanAcidfication > 35)
            {
                healthDepletionCoEff += 4f;
            }
            if (m_OceanAcidfication > 40)
            {
                healthDepletionCoEff += 5f;
            }

            if (m_OceanAcidfication > 10)
            {
                EventSystem.instance.RaiseEvent(new OnHealth { deltaHealthAmt = -healthDepletionCoEff * Time.deltaTime });
                if (!playedAcidAudio)
                {
                    EventSystem.instance.RaiseEvent(new AcidityBleachAudio
                    {
                        type = "acid"
                    });
                    playedAcidAudio = true;
                }


            }

            EventSystem.instance.RaiseEvent(new UpdateOceanAcidificationUI { oceanAcidicfaction = m_OceanAcidfication });
            #endregion
        
        if(alive == false)
        {
            EventSystem.instance.RaiseEvent(new CheckForRestart{ });
        }

    }

    void ChangeOceanAcidfcation(OnFinishEvent onFinishObj)
    {
        finished2Objectives = true;
        if (alive)
        {
            m_OceanAcidfication += 2.5f;

        }
    }

    void OnPlayerDied(PlayerDie playerDie)
    {
        alive = false;
    }

    void AddPoints(UpdatePoints updatePoints)
    {
        points += updatePoints.deltaPoints;
    }

    void UpdateCurrentInput(ChangeInputType inputType)
    {
        Platform = inputType.platform;
    }

    private void PauseGame(PauseGame _pause)
    {
        pause = !pause;
    }
    
}
