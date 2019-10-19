using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public class Mouth : MonoBehaviour
{
    public SphereCollider range;
    public Player _Player;

    public ObjectiveImages currSpecies;
    public string[] FoodSpecies;
    private void Start()
    {
//        FoodSpecies = (string[])System.Enum.GetNames(typeof(ObjectiveImages));
    }
    private void Awake()
    {
        EventSystem.instance.AddListener<UpdateObjectiveUI>(UpdateCurrSpecies);
    }
    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<UpdateObjectiveUI>(UpdateCurrSpecies);
    }

    void OnTriggerStay(Collider col)
    {
        
        if (col.tag == "Eatable" && (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Joystick1Button2)))
        {
            Debug.Log("Collided with me food and trying to eat");
            if (col.gameObject.GetComponent<Eatable>().species == currSpecies)
            {
                 Debug.Log("Eating correct food");
                Destroy(col.gameObject);

                EventSystem.instance.RaiseEvent(new AteFood { });
                if (_Player.health > 76)
                {
                    _Player.health = 100.0f;
                }
                else
                {
                    _Player.health += 15.0f;

                }
                // Debug.Log("prey: " + col.gameObject);
            }


        }
    }
    private void UpdateCurrSpecies(UpdateObjectiveUI updateObjectiveUI)
    {
        currSpecies = updateObjectiveUI.image;
    }
}
