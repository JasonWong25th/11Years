using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objective")]
public class Objective : ScriptableObject
{
    public new string name;
    public string description;

    public int toEat;
    public int eaten;

    public bool isActive;

    public ObjectiveImages type;

}
