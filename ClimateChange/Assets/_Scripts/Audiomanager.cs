using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;
using AmbientSounds;

public class Audiomanager : MonoBehaviour
{
    #region Singleton
    private static Audiomanager _instance;
    public static Audiomanager Instance //Ensures that this is the only instance in the class
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Audiomanager();
            }
            return _instance;
        }
    }
    #endregion
    [SerializeField]
    private AudioSource narrationAudioSource;

    [Header("Narration Audio"), Space(5)]
    public AudioClip inital;
    public AudioClip inital2;
    public AudioClip inital3;
    public AudioClip acidity;
    public AudioClip bleach;

    [Header("Death Audio"), Space(5)]
    public AudioClip deadzones;
    public AudioClip eaten;
    public AudioClip acidification;
    public AudioClip starvation;
    public AudioClip fishnet;
    [Header("11Years and Political Change"),Space (1)]
    public AudioClip YearsAnd17Months;

    public AudioSource NarrationAS()
    {
        return narrationAudioSource;
    }

    private bool isPlayingDeathExplanation = false;

    private bool isCollectablePlaying = false;
    void Awake()
    {
        EventSystem.instance.AddListener<CollectableEvent>(OnPlayCollectableNarration);
        EventSystem.instance.AddListener<OnDismissCollectable>(OnDismissCollectableAudio);
        EventSystem.instance.AddListener<PlayerDie>(OnPlayerDied);
        EventSystem.instance.AddListener<AcidityBleachAudio>(OnAcidLimit);
        isPlayingDeathExplanation = false;
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<CollectableEvent>(OnPlayCollectableNarration);
        EventSystem.instance.RemoveListener<OnDismissCollectable>(OnDismissCollectableAudio);
        EventSystem.instance.RemoveListener<PlayerDie>(OnPlayerDied);
        EventSystem.instance.RemoveListener<AcidityBleachAudio>(OnAcidLimit);

    }
    private void Start()
    {
        Invoke("InitalEducationAudio", 5);
    }
    void InitalEducationAudio()
    {
        AmbienceManager.Play(inital, 1);
        AmbienceManager.ActivateEvent("LowerBackground");
        Invoke("NextEducationAudio", 4);
    }
    void NextEducationAudio()
    {
        AmbienceManager.PausePlayback();
        AmbienceManager.Play(inital2, 1);
        AmbienceManager.ContinuePlayback();
        Invoke("LastInitialEducationAudio", 11);
    }
    void LastInitialEducationAudio()
    {
        AmbienceManager.PausePlayback();
        AmbienceManager.Play(inital3, 1);
        AmbienceManager.ContinuePlayback();
        Invoke("RaiseBackgroundNoise", 5);
    }
    void RaiseBackgroundNoise()
    {
        AmbienceManager.DeactivateEvent("LowerBackground");
    }

    void OnDismissCollectableAudio(OnDismissCollectable onDismissCollectable)
    {
        if (isCollectablePlaying)
        {
            narrationAudioSource.Stop();
            isCollectablePlaying = false;
        }
        AmbienceManager.Enable();
        AmbienceManager.ContinuePlayback();
    }

    void OnPlayCollectableNarration(CollectableEvent collectableEvent)
    {
        AmbienceManager.Disable();
        
        narrationAudioSource.Pause();
        AmbienceManager.PausePlayback();
        narrationAudioSource.clip = collectableEvent.recording;
        narrationAudioSource.Play();
        
        isCollectablePlaying = true;
    }

    void OnPlayerDied(PlayerDie player)
    {
        if (!isPlayingDeathExplanation)
        {
            narrationAudioSource.Pause();
            switch (player.causeOfDeath)
            {
                case CausesOfDeath.Acidification:
                    narrationAudioSource.clip = acidification;
                    break;
                case CausesOfDeath.Deadzones:
                    narrationAudioSource.clip = deadzones;
                    break;
                case CausesOfDeath.Eaten:
                    narrationAudioSource.clip = eaten;
                    break;
                case CausesOfDeath.Fishnets:
                    narrationAudioSource.clip = fishnet;
                    break;
                case CausesOfDeath.Hunger:
                    narrationAudioSource.clip = null;
                    break;
            }
            AmbienceManager.ActivateEvent("LowerBackground");
            narrationAudioSource.Play();
            Invoke("Play11YearsAudio", 5);
            isPlayingDeathExplanation = true;
        }
    }
    void Play11YearsAudio()
    {
        narrationAudioSource.clip = YearsAnd17Months;
        narrationAudioSource.Play();
        Invoke("RaiseBackgroundNoise", 10);
    }
    void OnAcidLimit(AcidityBleachAudio acidityBleachAudio)
    {
        narrationAudioSource.Pause();
        if (acidityBleachAudio.type == "acid")
        {
            narrationAudioSource.clip = acidity;
        }
        else
        {
            narrationAudioSource.clip = bleach;
        }
        narrationAudioSource.Play();
    }
}
