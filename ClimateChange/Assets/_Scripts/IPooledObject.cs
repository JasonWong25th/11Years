using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    void OnSpawnObject();//Equivalent of an OnEnable method everytime you respawn it since ONEnable don't trigger when you set it to active again
}
