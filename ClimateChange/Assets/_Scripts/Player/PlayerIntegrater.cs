using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LowPolyAnimalPack;
using GH;

public class PlayerIntegrater : WanderScript
{
    private void Start()
    {
        characterController.enabled = false;
        isPlayer = true;
    }
    public override void NonNavMeshRunAwayFromAnimal(WanderScript predator)
    {
        //signal to player through UI that enemy is near...
        //base.NonNavMeshRunAwayFromAnimal(predator);
    }

    public override void RunAwayFromAnimal(WanderScript predator)
    {
        //signal to player enemy is near...
        //base.RunAwayFromAnimal(predator);
    }

    public override void BeginIdleState(bool firstState = false)
    {
        //base.BeginIdleState(firstState);
    }
    public override void Die()
    {
        //base.Die();
        EventSystem.instance.RaiseEvent(new PlayerDie { causeOfDeath = CausesOfDeath.Eaten});
    }
}
