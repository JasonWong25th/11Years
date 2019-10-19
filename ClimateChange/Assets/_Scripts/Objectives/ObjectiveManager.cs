using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GH;

public enum ObjectiveImages
{
    ZooPlankton,
    Shrimp,
    Mullocus,
    Crayfish,
    Crabs
}
public class OnFinishEvent : GH.Event
{
}
public class UpdateObjectiveUI : GH.Event
{
    public Objective objective;
    public ObjectiveImages image;
}
public class AteFood : GH.Event
{

}
public class ObjectiveManager : MonoBehaviour
{
    public ObjectivesDataBase database;
    List<Objective> objectives = new List<Objective>();

    [Header("Current Objective Info"), Space(5)]
    public Objective currObjective;
    public int eaten;
    public int toEat;

    int currIndex = 0;

    private void Awake()
    {
        EventSystem.instance.AddListener<AteFood>(UpdateObjective);
        LoadDataBase(database);
        currObjective = objectives[currIndex];
        UpdateObjectiveUI();
    }

    private void OnDisable()
    {
        EventSystem.instance.RemoveListener<AteFood>(UpdateObjective);
        CleanDataBase(database);
    }

    private void Update()
    {
        eaten = currObjective.eaten;
        toEat = currObjective.toEat;

        if (eaten == toEat)
        {
            //When you finish the current Objective add this line
            
            currObjective.eaten = 0;

            currIndex++;
            currObjective = objectives[currIndex];
            UpdateObjectiveUI();
        }

        //simulates the action of "eating"
        /*if (Input.GetKeyUp(KeyCode.Space)) 
        {
            currObjective.eaten++;
            UpdateObjectiveUI();
        }
        */
//        Debug.Log("currentObjective: " + currObjective.name);
        
    }

    void LoadDataBase(ObjectivesDataBase newDataBase)
    {
       for(int i=0; i<newDataBase.objectives.Length; i++)
        {
            if(newDataBase.objectives[i] != null)
            {
                objectives.Add(newDataBase.objectives[i]);
            }         
        }
    }

    void CleanDataBase(ObjectivesDataBase DataBase)
    {
        for (int i = 0; i < DataBase.objectives.Length; i++)
        {
            if (DataBase.objectives[i] != null)
            {
                objectives[i].eaten = 0;
            }
        }
    }

    void UpdateObjective(AteFood ate)
    {
        currObjective.eaten++;
        if(currObjective.eaten >= currObjective.toEat)
        {
            currObjective = objectives[currIndex++];
            if(currIndex > 1)
            {
                EventSystem.instance.RaiseEvent(new OnFinishEvent { });
            }
        }
        UpdateObjectiveUI();
    }

    void UpdateObjectiveUI()
    {
        //should send new objective info to UI
        EventSystem.instance.RaiseEvent(new UpdateObjectiveUI{objective = currObjective, image = currObjective.type});
    }
}
