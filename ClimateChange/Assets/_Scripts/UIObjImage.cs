using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GH;

public class UIObjImage : MonoBehaviour
{
    [SerializeField]
    private Sprite ZooPlankton;
    [SerializeField]
    private Sprite Shrimp;
    [SerializeField]
    private Sprite Crabs;
    [SerializeField]
    private Sprite Crayfish;
    [SerializeField]
    private Sprite Mullocus;

    private Image thisImage;
    private void Awake()
    {
        EventSystem.instance.AddListener<UpdateObjectiveUI>(UpdateObjImageUI);
        thisImage = GetComponent<Image>();
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<UpdateObjectiveUI>(UpdateObjImageUI);
    }

    void UpdateObjImageUI(UpdateObjectiveUI updateObjectiveUI)
    {
        switch (updateObjectiveUI.image)
        {
            case (ObjectiveImages.ZooPlankton):
                thisImage.sprite = ZooPlankton;
                break;
            case ObjectiveImages.Shrimp:
                thisImage.sprite = Shrimp;
                break;
            case ObjectiveImages.Crabs:
                thisImage.sprite = Crabs;
                break;
            case ObjectiveImages.Crayfish:
                thisImage.sprite = Crayfish;
                break;
            case ObjectiveImages.Mullocus:
                thisImage.sprite = Mullocus;
                break;
        }
    }
}
