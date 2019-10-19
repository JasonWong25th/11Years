using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using GH;

public class DeathUI: GH.Event
{
    public CausesOfDeath cause;
}

public class PauseUI: GH.Event
{
    public bool toggle;
}

public class OnDismissCollectable : GH.Event
{

}

public class UIManager : MonoBehaviour
{
    public bool startTimer;
    public float timer = .5f;
    public float currTime;

    [Header("Gameplay"), Space(5)]
    [SerializeField]
    private Canvas gameCanvas;
    public Animator pauseAnimator;
    public Animator objectivesAnimator;
    public TMPro.TextMeshProUGUI objectiveDescription;
    public TMPro.TextMeshProUGUI objectiveStatus;
    public Slider oceanAcidifcationSlider;
    public Slider healthSlider;

    [Header("Narration"), Space(5)]
    [SerializeField]
    private Canvas narrationCanvas;
    public TMPro.TextMeshProUGUI subtitleText;

    [Header("Death"), Space(5)]
    [SerializeField]
    private GameObject deathCanvasContent;
    private Animator deathAnim;
    [SerializeField]
    private TextMeshProUGUI text_explanation;

    private bool ifCollectableOn = false;
    private bool updatedDeathText = false;

    private void Awake()
    {
        EventSystem.instance.AddListener<UpdateObjectiveUI>(UpdateObjUI);
        EventSystem.instance.AddListener<UpdateOceanAcidificationUI>(UpdateOceanAcidifcationUI);
        EventSystem.instance.AddListener<UpdatePlayerUI>(UpdatePlayerStatsUi);
        EventSystem.instance.AddListener<CollectableEvent>(UpdateSubtitle);
        EventSystem.instance.AddListener<CollectableEvent>(ToggleCanvasCollectable);
        EventSystem.instance.AddListener<DeathUI>(ToggleDeathCanvas);
        EventSystem.instance.AddListener<PauseGame>(TogglePause);

        EventSystem.instance.AddListener<OnDismissCollectable>(DismissUI);

        deathAnim = deathCanvasContent.GetComponent<Animator>();

        if(text_explanation == null)
        {
            Debug.LogError("Text Explanation is Unassigned", this);
        }
        updatedDeathText = false;
    }

    private void Start()
    {
        objectivesAnimator.SetBool("stayStill", true);
    }

    private void Update()
    {
        if (startTimer)
        {
           // StartObjectivesTimer();
        
        }
    }

    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<UpdateOceanAcidificationUI>(UpdateOceanAcidifcationUI);
        EventSystem.instance.RemoveListener<UpdateObjectiveUI>(UpdateObjUI);
        EventSystem.instance.RemoveListener<UpdatePlayerUI>(UpdatePlayerStatsUi);
        EventSystem.instance.RemoveListener<CollectableEvent>(UpdateSubtitle);
        EventSystem.instance.RemoveListener<CollectableEvent>(ToggleCanvasCollectable);
        EventSystem.instance.RemoveListener<DeathUI>(ToggleDeathCanvas);
        EventSystem.instance.RemoveListener<OnDismissCollectable>(DismissUI);
        EventSystem.instance.RemoveListener<PauseGame>(TogglePause);

    }

    void UpdateObjUI(UpdateObjectiveUI obj)
    {

        startTimer = true;
        objectiveDescription.SetText(obj.objective.description); 
        objectiveStatus.SetText(obj.objective.eaten + " / " + obj.objective.toEat);
        
    }

    void UpdateOceanAcidifcationUI(UpdateOceanAcidificationUI acidificationUI)
    {
        oceanAcidifcationSlider.value = acidificationUI.oceanAcidicfaction;
    }

    void UpdatePlayerStatsUi(UpdatePlayerUI updatePlayerUI)
    {
        healthSlider.value = updatePlayerUI.healhpoints;
    }

    void UpdateSubtitle(CollectableEvent collectableUIEvent)
    {
        subtitleText.SetText(collectableUIEvent.subtitleText);
    }

    void ToggleCanvasCollectable(CollectableEvent collectableEvent)
    {
        narrationCanvas.enabled = true;
        ifCollectableOn = true;
    }

    void DismissUI(OnDismissCollectable onDismissCollect)
    {
        if (ifCollectableOn)
        {
            gameCanvas.enabled = true;
            narrationCanvas.enabled = false;
            narrationCanvas.enabled = false;
            ifCollectableOn = false;
        }

    }

    void TogglePause(PauseGame pause)
    {
        pauseAnimator.SetBool("pause", pause.pause);
    }

    void ToggleDeathCanvas(DeathUI death)
    {
        gameCanvas.enabled = false;
        narrationCanvas.enabled = false;
        deathCanvasContent.SetActive(true);

        deathAnim.SetBool("Dead", true);
        if (!updatedDeathText)
        {
            string deathcause = "";
            switch (death.cause)
            {
                case CausesOfDeath.Acidification:
                    deathcause = "Acidification";
                    break;
                case CausesOfDeath.Deadzones:
                    deathcause = "Deadzones";
                    break;
                case CausesOfDeath.Eaten:
                    deathcause = "Eaten";
                    break;
                case CausesOfDeath.Fishnets:
                    deathcause = "Fishnets";
                    break;
                case CausesOfDeath.Hunger:
                    deathcause = "Hunger";
                    break;
            }
            text_explanation.text += deathcause;
            updatedDeathText = true;
        }
    }

    void StartObjectivesTimer()
    {
        if (startTimer)
        {
            if (currTime < timer)
            {
                currTime += Time.deltaTime;
                //objectivesAnimator.SetBool("stayStill", false);
            }
            else
            {
                currTime = 0f;
                //objectivesAnimator.SetBool("stayStill", true);
                startTimer = false;
            }
        }    
    }
}
